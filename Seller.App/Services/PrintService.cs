using BLL.Dtos.TransactionDtos;
using ESC_POS_USB_NET.Printer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Seller.App.Services;

public class PrintService : IDisposable
{
    public string printerName { get; set; } = string.Empty;
    Printer printer;
    public PrintService()
	{
		//initialize print name
	}

	public void Print(List<TransactionDto> transactions, string sellerFullName, int orderId)
	{
		printer = new Printer(printerName, "UTF-8");
        Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        printer.Separator();
        Bitmap image = new Bitmap(Bitmap.FromFile("logo.png"));
        printer.Image(image);
        printer.AlignCenter();
        printer.DoubleWidth2();
        printer.Append("\"MDevs Group LLC\"");
        printer.Separator();
        printer.Append("\n");

        printer.AlignLeft();
        printer.Append("t/r Nomi                Miqdori     Jami narxi");
        printer.Separator();

        int tr = 1;
        decimal sum = 0;
        printer.Append("\n");
        foreach (var item in transactions)
        {
            string text = $"{tr}.  {item.Name}";
            int strLength = 32 - text.Length;
            for (int i = 1; i <= strLength; i++)
            {
                text += " ";
            }
            string temp = $"{item.Quantity}*{item.Price}";
            text += temp;
            strLength = 16 - temp.Length;
            for (int i = 0; i < strLength; i++)
            {
                text += " ";
            }
            text += item.TotalPrice.ToString();
            printer.CondensedMode(text);
            printer.Append("\n");
            tr++;
            sum += item.TotalPrice;
        }

        printer.Separator();
        printer.AlignLeft();
        printer.Append("\n");
        printer.BoldMode($"Jami summa:                   {sum} so'm");
        printer.Append("\n");

        printer.Separator();
        printer.AlignLeft();
        printer.Append("\n");
        printer.Append("Sotuvchi:                  Nodirbek Abdulaxadov");
        printer.AlignRight();
        printer.Append("\n");
        printer.AlignLeft();
        printer.Append("Sana:                      " + DateTime.Now.ToString());


        string barcode = (100000000000 + orderId).ToString();


        printer.Append("\n");
        printer.AlignCenter();
        printer.Code39(barcode);
        printer.Append("\n");

        printer.AlignCenter();
        printer.BoldMode("Xaridingiz uchun tashakkur!");

        printer.Append("\n");
        printer.Separator();


        printer.FullPaperCut();
        printer.PrintDocument();
    }

    public void Dispose()
         => GC.SuppressFinalize(this);
}