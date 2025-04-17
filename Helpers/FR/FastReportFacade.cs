// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.FastReportFacade
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FastReport;
using FastReport.Export;
using FastReport.Export.Pdf;
using FastReport.Export.Text;
using FastReport.Export.Zpl;
using FastReport.Utils;
using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Helpers.FR
{
  public class FastReportFacade
  {
    private string _textPrint;
    private string _qrPrint;

    public string SelectTemplate(ReportType reportType, Gbs.Core.Entities.Users.User user)
    {
      return System.Windows.Application.Current?.Dispatcher?.Invoke<string>((Func<string>) (() =>
      {
        (string Path, bool Result) templateFr = new FrmSelectedTemplateFR().GetTemplateFR(reportType.Directory.FullName, user);
        string path = templateFr.Path;
        return !templateFr.Result ? (string) null : path;
      }));
    }

    public void SelectTemplateAndShowReport(IPrintableReport report, Gbs.Core.Entities.Users.User user, string path = null)
    {
      try
      {
        System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
        {
          if (string.IsNullOrEmpty(path))
          {
            (string Path, bool Result) templateFr = new FrmSelectedTemplateFR().GetTemplateFR(report.Type.Directory.FullName, user);
            string path1 = templateFr.Path;
            if (!templateFr.Result)
              return;
            path = path1;
          }
          LogHelper.Debug("Печать отчета " + path);
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.MasterReportViewModel_Подготовка_данных_для_отчета);
          Report report1 = this.PrepareReport(path, report.DataDictionary, report.Properties);
          if (report1 == null)
          {
            LogHelper.Debug("Экземпляр отчета не ссылкается на объект");
          }
          else
          {
            report1.Show(false);
            progressBar.Close();
          }
        }));
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        switch (ex)
        {
          case DataTableException _:
          case CompilerException _:
            LogHelper.Error(ex, "Ошибка печати документа", false);
            MessageBoxHelper.Error(string.Format(Translate.FastReportFacade_Возникла_ошибка_печати_документа___0_Вероятно__документ_имеет_несовместимый_формат_или_в_его_структуре_допущена_ошибка_, (object) Other.NewLine()));
            break;
          default:
            LogHelper.Error(ex, "Ошибка печати документа");
            break;
        }
      }
    }

    public void PrintReport(IPrintableReport printableReport, string printerName = null, string path = null)
    {
      if (path == null)
        path = this.SelectTemplate(printableReport.Type, (Gbs.Core.Entities.Users.User) null);
      if (string.IsNullOrEmpty(path))
        return;
      using (Report report = this.PrepareReport(path, printableReport.DataDictionary, printableReport.Properties))
      {
        report.PrintSettings.Printer = printerName ?? string.Empty;
        report.PrintSettings.ShowDialog = string.IsNullOrEmpty(printerName);
        report.Prepare();
        report.Print();
      }
    }

    public void PrintText(string text, string printerName = null, string qr = "")
    {
      this._textPrint = text;
      this._qrPrint = qr;
      PrintDocument printDocument = new PrintDocument();
      PrintDialog printDialog = new PrintDialog();
      if (printerName == null)
      {
        if (printDialog.ShowDialog() != DialogResult.OK)
          return;
        printerName = printDialog.PrinterSettings.PrinterName;
      }
      PrinterSettings printerSettings = new PrinterSettings()
      {
        PrinterName = printerName
      };
      printDocument.PrinterSettings = printerSettings;
      printDocument.PrintPage += new PrintPageEventHandler(this.PD_PrintPage);
      printDialog.Document = printDocument;
      printDialog.Document.Print();
    }

    public void PrintFile(string pathFile, string printerName = null)
    {
      PrintDialog printDialog = new PrintDialog();
      if (printerName == null)
      {
        if (printDialog.ShowDialog() != DialogResult.OK)
          return;
        printerName = printDialog.PrinterSettings.PrinterName;
      }
      FileStream fileStream = new FileStream("file.txt", FileMode.OpenOrCreate);
      TextExportPrint.PrintStream(printerName, Translate.FastReportFacade_Печать_из_PDF_файла, 1, (Stream) fileStream);
    }

    private void PD_PrintPage(object sender, PrintPageEventArgs e)
    {
      int x = 0;
      int y = 0;
      PrintDocument printDocument = new PrintDocument();
      if (this._qrPrint != "")
      {
        BitmapImage qrCode = this._qrPrint.GetQrCode(30, 30);
        e.Graphics.DrawImage((Image) ImagesHelpers.BitmapImage2Bitmap(qrCode), 5, 5);
        y += 120;
        x += 20;
      }
      e.Graphics.DrawString(this._textPrint, new Font("Calibri", 8f), Brushes.Black, (float) x, (float) y);
    }

    public void PrintZplLable(
      IPrintableReport printableReport,
      ZplSetting setting,
      bool isShowPreview,
      string printerName = null)
    {
      string reportFilePath = this.SelectTemplate(printableReport.Type, new Gbs.Core.Entities.Users.User());
      if (string.IsNullOrEmpty(reportFilePath))
        return;
      using (Report report = this.PrepareReport(reportFilePath, printableReport.DataDictionary, printableReport.Properties))
      {
        if (isShowPreview)
          System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
          {
            EnvironmentSettings environmentSettings = new EnvironmentSettings()
            {
              PreviewSettings = {
                Buttons = PreviewButtons.Save,
                ShowInTaskbar = true
              },
              ReportSettings = {
                ShowProgress = false
              }
            };
            report.Show(true);
          }));
        PrinterSettings printerSettings = new PrinterSettings()
        {
          PrinterName = printerName ?? string.Empty
        };
        if (string.IsNullOrEmpty(printerName) && !report.ShowPrintDialog(out printerSettings))
          return;
        report.Prepare();
        using (MemoryStream memoryStream = new MemoryStream())
        {
          new ZplExport()
          {
            Density = ((ZplExport.ZplDensity) setting.Density),
            PrintAsBitmap = setting.IsPrintAsBitmap,
            CodePage = "^CI28",
            PrinterFont = "@N,44,30,E:TT0003M_.TTF"
          }.Export(report, (Stream) memoryStream);
          System.Text.Encoding.UTF8.GetString(memoryStream.GetBuffer());
          TextExportPrint.PrintStream(printerSettings.PrinterName, Translate.Devices_ZPL_печать, (int) printerSettings.Copies, (Stream) memoryStream);
        }
      }
    }

    private static string ZebraEncode(string text)
    {
      StringBuilder stringBuilder = new StringBuilder();
      Dictionary<char, string> dictionary = new Dictionary<char, string>();
      foreach (char key in text)
      {
        if (!dictionary.ContainsKey(key))
        {
          byte[] bytes = System.Text.Encoding.UTF8.GetBytes(key.ToString());
          if (bytes.Length > 1)
          {
            string str = ((IEnumerable<byte>) bytes).Aggregate<byte, string>(string.Empty, (Func<string, byte, string>) ((current, b) => current + "_" + BitConverter.ToString(new byte[1]
            {
              b
            }).ToLower()));
            dictionary[key] = str;
          }
          else
            dictionary[key] = key.ToString();
          stringBuilder.Append(dictionary[key]);
        }
        else
          stringBuilder.Append(dictionary[key]);
      }
      return stringBuilder.ToString();
    }

    public bool SaveReport(IPrintableReport printableReport, string pathSave)
    {
      try
      {
        using (Report report = this.PrepareReport(((IEnumerable<FileInfo>) ReportType.EmailOrders.Directory.GetFiles()).FirstOrDefault<FileInfo>()?.FullName, printableReport.DataDictionary, printableReport.Properties, true))
        {
          if (report == null || !report.Report.Prepare())
            return false;
          PDFExport pdfExport = new PDFExport();
          pdfExport.ShowProgress = false;
          pdfExport.Compressed = true;
          pdfExport.AllowPrint = true;
          pdfExport.EmbeddingFonts = true;
          PDFExport export = pdfExport;
          report.Export((ExportBase) export, pathSave);
          report.Dispose();
          export.Dispose();
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при подготовке файла-отчета в формате PDF");
        return false;
      }
    }

    public void ShowDesigner(string reportFilePath)
    {
      Report report = this.PrepareReport(reportFilePath, (Dictionary<string, IEnumerable>) null, (IEnumerable<IReportProperty>) null, true);
      new EnvironmentSettings().DesignerSettings.ShowInTaskbar = true;
      report.Design(false);
    }

    public Report PrepareReport(
      string reportFilePath,
      Dictionary<string, IEnumerable> dataSet,
      IEnumerable<IReportProperty> parameters,
      bool isSave = false)
    {
      try
      {
        if (string.IsNullOrEmpty(reportFilePath))
        {
          LogHelper.Debug("Не указан путь к файлу отчета");
          return (Report) null;
        }
        EnvironmentSettings environmentSettings = new EnvironmentSettings()
        {
          PreviewSettings = {
            Buttons = PreviewButtons.Print | PreviewButtons.Save | PreviewButtons.PageSetup,
            ShowInTaskbar = true
          },
          ReportSettings = {
            ShowProgress = false
          }
        };
        Report report = new Report();
        report.Load(reportFilePath);
        if (dataSet != null)
        {
          foreach (KeyValuePair<string, IEnumerable> data in dataSet)
            report.RegisterData(data.Value, data.Key);
        }
        if (parameters != null)
        {
          foreach (IReportProperty parameter in parameters)
            report.SetParameterValue(parameter.Name, parameter.Value);
        }
        if (new ConfigsRepository<Settings>().Get().Interface.IsVisibilityDesign && !isSave)
          System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() => report.Design()));
        return report;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка подготовки отчета");
        return (Report) null;
      }
    }

    public void CreateTemplatesFolders()
    {
      foreach (ReportType reportType in ReportType.GetAll())
        FileSystemHelper.ExistsOrCreateFolder(reportType.Directory.FullName);
    }

    public static void DownloadTemplatesFromServer()
    {
      try
      {
        string str1 = FileSystemHelper.TempFolderPath();
        NetworkHelper.UrlPath pingedUrl = NetworkHelper.GetUrlPaths().Where<NetworkHelper.UrlPath>((Func<NetworkHelper.UrlPath, bool>) (x => x.UrlType == NetworkHelper.UrlTypes.FRTemplates)).GetPingedUrl();
        if (pingedUrl == null)
        {
          LogHelper.Debug("Не удалось определить сервер для загрузки шаблонов");
        }
        else
        {
          string str2 = Path.Combine(str1, "templatesFR.zip");
          if (!NetworkHelper.DownloadFile(pingedUrl.Url?.ToString() + "templatesFR.zip", str2))
          {
            LogHelper.Debug("Не удалось скачать архив с шаблонами с сервера");
          }
          else
          {
            ZipFile.ExtractToDirectory(str2, str1, System.Text.Encoding.UTF8);
            foreach (DirectoryInfo directoryInfo1 in ((IEnumerable<string>) Directory.GetDirectories(str1)).Select<string, DirectoryInfo>((Func<string, DirectoryInfo>) (x => new DirectoryInfo(x))).ToList<DirectoryInfo>())
            {
              DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo1.FullName.Replace(str1, ApplicationInfo.GetInstance().Paths.TemplatesFrPath));
              if (directoryInfo2.Exists && !((IEnumerable<FileInfo>) directoryInfo2.GetFiles("*.frx", SearchOption.AllDirectories)).Any<FileInfo>())
                FileSystemHelper.CopyFolder(directoryInfo1.FullName, directoryInfo2.FullName, false);
            }
            Directory.Delete(str1, true);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка загрузки шаблонов");
      }
    }
  }
}
