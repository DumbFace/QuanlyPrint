using Microsoft.Office.Interop.Word;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using QuanLyMayIn.Hubs;
using QuanLyMayIn.Models;
using System.Web.UI.WebControls;
using Document = iTextSharp.text.Document;
using Spire.Xls;
using Image = iTextSharp.text.Image;
    
using System.Text;

namespace QuanLyMayIn.Controllers
{
    [Authorize(Roles = "ADMIN, NHAN_VIEN")]
    public class NV_BanInController : Controller
    {

        
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();
 
        // GET: NV_BanIn
        public ActionResult Index()
        {
            if (user.CheckQuyen("NV_BanIn", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

        public ActionResult BanIn(string id)
        {
            if (user.CheckQuyen("NV_BanIn", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            var TenTaiLieu = db.NV_BanIn.Where(_ => _.MaTaiLieu == id).FirstOrDefault().TenTaiLieu;
            ViewBag.TenFile = TenTaiLieu;
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_BanIn,TenTaiLieu,MaTaiLieu,ID_NhanVien,ThoiGianUpload,ThoiGianPrint,TrangThai,TrangThaiText,TongSoTrang,TongSoTrangDaIn,JobID,TenMayIn")] NV_BanIn nV_BanIn, HttpPostedFileBase BanIn)
        {
            var filename1 = System.Guid.NewGuid().ToString();
            var filename2 = System.Guid.NewGuid().ToString();
            var filename3 = System.Guid.NewGuid().ToString();

            // luu file vao server 
            string path1 = Path.Combine(Server.MapPath("~/Content/uploads"), BanIn.FileName);
            BanIn.SaveAs(path1);
            
            var t = Path.GetExtension(path1);

            string path2 = Path.Combine(Server.MapPath("~/Content/uploads"), filename2 + ".pdf");
          


            string path3 = Path.Combine(Server.MapPath("~/Content/uploads"), filename3 + ".pdf");

            if (t == ".docx" || t == ".doc")
            {
               
               
            }
            else if (t == ".xlsx")
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path1);

                Worksheet sheet = workbook.Worksheets[0];
                sheet.SaveToPdf(path2);
            }
            else if (t == ".pdf")
            {
                path2 = path1;
            }else if (t == ".jpg" || t == ".png" || t == ".JPG" || t == ".PNG")
            {
                Document document = new Document();
                using (var stream = new FileStream(path2, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter.GetInstance(document, stream);
                    document.Open();
                    using (var imageStream = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var image = Image.GetInstance(imageStream);
                        float maxWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; float maxHeight = document.PageSize.Height - document.TopMargin - document.BottomMargin; if (image.Height > maxHeight || image.Width > maxWidth) image.ScaleToFit(maxWidth, maxHeight);

                        document.Add(image);
                    }
                    document.Close();
                }
            }

            PdfReader reader = new PdfReader(path2);
            FileStream fs = new FileStream(path3, FileMode.Create, FileAccess.Write, FileShare.None);
            
            using (PdfStamper stamper = new PdfStamper(reader, fs))
            {
                Dictionary<String, String> info = reader.Info;
                info.Remove("Title");
                info.Add("Title", filename3 + ".pdf");
                stamper.MoreInfo = info;
                nV_BanIn.TenTaiLieu = filename3 + ".pdf";
                nV_BanIn.TongSoTrang = reader.NumberOfPages;
                nV_BanIn.TenTaiLieuDinhKem = BanIn.FileName;
                nV_BanIn.MaTaiLieu = System.Guid.NewGuid().ToString();
                nV_BanIn.ID_NhanVien = user.GetUser();
                nV_BanIn.ThoiGianUpload = DateTime.Now;
                nV_BanIn.TrangThai = false;
                nV_BanIn.TrangThaiText = "Chưa Được In";
                nV_BanIn.TongSoTrangDaIn = 0;
                nV_BanIn.JobID = 0;
                nV_BanIn.TenMayIn = "";
            }
     
            db.NV_BanIn.Add(nV_BanIn);
            db.SaveChanges();
            return Json(new { status = "success", title = "thành công", message = "Đã Tạo Thành Công Bản In" }, JsonRequestBehavior.AllowGet);
          
        }


        //public static void Convertwordtopdf(string input, string output, WdSaveFormat format)
        //{
        //    // Create an instance of Word.exe
        //    Word._Application oWord = new Word.Application();

        //    // Make this instance of word invisible (Can still see it in the taskmgr).
        //    oWord.Visible = false;

        //    // Interop requires objects.
        //    object oMissing = System.Reflection.Missing.Value;
        //    object isVisible = true;
        //    object readOnly = false;
        //    object oInput = input;
        //    object oOutput = output;
        //    object oFormat = format;

        //    // Load a document into our instance of word.exe
        //    Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

        //    // Make this document the active document.
        //    oDoc.Activate();

        //    // Save this document in Word 2003 format.
        //    oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

        //    // Always close Word.exe.
        //    oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
        //}

        public ActionResult LoadData()
        {
            var ID_NhanVien = user.GetUser(); 

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Print_LimitConnection"].ConnectionString))
            {
                connection.Open();
                var commandsql = "SELECT [ID_BanIn],[TrangThai],[TrangThaiText],[TenMayIn],[ThoiGianPrint],[TongSoTrangDaIn],[PaperSize] FROM dbo.NV_BanIn Where ID_NhanVien = " + ID_NhanVien;
                using (SqlCommand command = new SqlCommand(commandsql, connection))
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;
                    var search = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
                    var UserID = user.GetUser();
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    var listBanIn = reader.Cast<IDataRecord>()
                            .Select(x => new
                            {
                                ID_BanIn = (int)x["ID_BanIn"],                             
                            }).ToList();

                    var data = (from bi1 in listBanIn
                                join bi in db.NV_BanIn on bi1.ID_BanIn equals bi.ID_BanIn
                                join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                                join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                                where nv.ID_NhanVien == ID_NhanVien 
                                orderby bi.ThoiGianPrint descending
                                select new { bi.ID_NhanVien, nv.TenNhanVien, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomNhanVien, bi.TenMayIn, bi.PaperSize }).ToList();


                    if (!string.IsNullOrEmpty(search))
                    {
                        data = data.Where(_ => _.TenTaiLieuDinhKem.ToUpper().Contains(search.ToUpper())).ToList();
                    }

                    recordsTotal = data.Count();
                    var data1 = data.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                MyHub.ReloadData();
            }
            
        }

        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }
    }
}
