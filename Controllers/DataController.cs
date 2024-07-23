using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CentralisationV0.Models.Entities;
using CentralisationdeDonnee.Models; // Assurez-vous que l'espace de noms correspondant est importé


using CentralisationV0.Services; // Ajout de l'espace de noms pour le service de données

namespace CentralisationV0.Controllers
{
    public class DataController : Controller
    {
        private readonly IDataService _dataService;
        private CentralisationContext db = new CentralisationContext();

        public DataController()
        {
            _dataService = new DataService();
        }

        // GET: Data
        public ActionResult Index()
        {
            var dataList = _dataService.GetData();
            ViewBag.Themes = db.Themes.ToList(); // Passer les thèmes à la vue
            return View("~/Views/Donnee/Index.cshtml", dataList);
        }

        // GET: Data/Create
        public ActionResult Create()
        {
            ViewBag.Themes = new SelectList(db.Themes, "IdTheme", "nom");
            return View();
        }

        // POST: Data/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,AcquisitionDate,PublicationDate,LastUpdatedDate,Description,Category,Telecommunication,ThemeId,Coverage,SpatialResolution,Summary,UrlData,Keywords,DataSize,ThemeName")] Data data)
        {
            if (ModelState.IsValid)
            {
                // Rechercher le thème par son nom
                var theme = db.Themes.FirstOrDefault(t => t.nom == data.ThemeName);

                // Si le thème n'existe pas, le créer
                if (theme == null)
                {
                    theme = new Theme
                    {
                        nom = data.ThemeName
                    };
                    db.Themes.Add(theme);
                    db.SaveChanges(); // Enregistrer le thème nouvellement créé dans la base de données
                }

                // Associer l'ID du thème à l'objet Data
                data.ThemeId = theme.IdTheme;

                // Ajouter l'objet Data à la base de données
                db.Datas.Add(data);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Les données ont été enregistrées avec succès.";
                return RedirectToAction("Index", "Donnee", new { success = true });
            }

            // Préparer la liste des thèmes pour la vue
            ViewBag.Themes = new SelectList(db.Themes, "IdTheme", "nom", data.ThemeId);
            return View(data);
        }
        // GET: Data/GetById/5
        public JsonResult GetById(int id)
        {
            var data = db.Datas.Include(d => d.Theme).FirstOrDefault(d => d.IdData == id);
            if (data == null)
            {
                return Json(new { success = false, message = "Donnée non trouvée" }, JsonRequestBehavior.AllowGet);
            }

            // Formater les dates au format ISO8601
            var isoAcquisitionDate = data.AcquisitionDate.ToString("yyyy-MM-ddTHH:mm:ss");
            var isoPublicationDate = data.PublicationDate.ToString("yyyy-MM-ddTHH:mm:ss");
            var isoLastUpdatedDate = data.LastUpdatedDate.ToString("yyyy-MM-ddTHH:mm:ss");

            // Construire l'objet JSON à retourner
            var jsonData = new
            {
                success = true,
                data = new
                {
                    IdData = data.IdData,
                    Title = data.Title,
                    AcquisitionDate = isoAcquisitionDate, // Format ISO8601
                    PublicationDate = isoPublicationDate, // Format ISO8601
                    LastUpdatedDate = isoLastUpdatedDate, // Format ISO8601
                    Description = data.Description,
                    Theme = data.Theme != null ? new { nom = data.Theme.nom } : null,
                    Coverage = data.Coverage,
                    SpatialResolution = data.SpatialResolution,
                    Summary = data.Summary,
                    Category = data.Category
                }
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(Data updatedData)
        {
            if (ModelState.IsValid)
            {
                var data = db.Datas.Include(d => d.Theme).FirstOrDefault(d => d.IdData == updatedData.IdData);

                if (data != null)
                {
                    data.Title = updatedData.Title;
                    data.AcquisitionDate = updatedData.AcquisitionDate;
                    data.PublicationDate = updatedData.PublicationDate;
                    data.LastUpdatedDate = updatedData.LastUpdatedDate;
                    data.Description = updatedData.Description;
                    data.Category = updatedData.Category;
                    data.Coverage = updatedData.Coverage;
                    data.SpatialResolution = updatedData.SpatialResolution;
                    data.Summary = updatedData.Summary;

                    // Update Theme
                    var theme = db.Themes.FirstOrDefault(t => t.nom == updatedData.ThemeName);
                    if (theme == null)
                    {
                        theme = new Theme
                        {
                            nom = updatedData.ThemeName
                        };
                        db.Themes.Add(theme);
                        db.SaveChanges();
                    }

                    data.ThemeId = theme.IdTheme;

                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true, message = "Donnée mise à jour avec succès." });
                }

                return Json(new { success = false, message = "Donnée non trouvée." });
            }

            return Json(new { success = false, message = "État du modèle invalide." });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var data = db.Datas.FirstOrDefault(d => d.IdData == id);
            if (data != null)
            {
                db.Datas.Remove(data);
                db.SaveChanges();
                return Json(new { success = true, message = "Donnée supprimée avec succès." });
            }
            return Json(new { success = false, message = "Donnée non trouvée." });
        }


    }
}
