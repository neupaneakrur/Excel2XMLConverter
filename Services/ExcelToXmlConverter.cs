using Converter.Models;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Converter.Services
{
    public class ExcelToXmlConverter
    {
        public void ConvertToXml(List<InformationModel> informations, string outputPath)
        {
            XDocument xmlDocument = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("Informations",
            new XElement("Header",
                new XElement("TallyData", "Input Data")
           ),
            new XElement("Data",
                from info in informations
                select new XElement("Row",
                    new XElement("Data", info.Date.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new XElement("Type", info.Type),
                    new XElement("Amount", info.Amount.ToString()),
                    new XElement("Currency", info.Currency),
                    new XElement("EventType", info.EventType),
                    new XElement("CreatedAt", info.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss"))
                    )
                 )
               )
             );
            xmlDocument.Save(outputPath);
        }
    }
}
