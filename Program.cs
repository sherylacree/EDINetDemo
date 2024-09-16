using EDINetDemo;
using indice.Edi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using indice.Edi.Tests.Models;

namespace EDINetDemo {


    class Program {
        private const string V = "N";

        static void Main(string[] args, EdiGrammar ediGrammar) {
            var po850 = buildPO850();
            serializePO850(po850, ediGrammar, ediGrammar.NewX12());
            //deserializePO850();

        }



        public static PurchaseOrder_850 buildPO850() {
            var po850 = new PurchaseOrder_850();

            var order = new PurchaseOrder_850.Order();
            order.PurchaseOrderNumber = "ANY001";
            order.PurchaseOrderDate = DateTime.Now;
            order.PurchaseOrderTypeCode = "NE";
            order.ReferenceIdentificationQualifier = "WO";
            order.ReferenceIdentification = "WO123456";
            order.CurrencyCode = "USD";

            var lineItem = new PurchaseOrder_850.OrderDetail();
            lineItem.OrderLineNumber = "010";
            lineItem.QuantityOrdered = 40;
            lineItem.UnitOfMeasurement = "EA";
            lineItem.UnitPrice = 19.99M;
            lineItem.BuyersPartNumber = "ABC123";
            lineItem.ProductDescription = "Digital Widget";
            lineItem.DeliveryRequestedDate = DateTime.Now;
            lineItem.ShipNoLaterDate = DateTime.Today;

            var dateRequested = new PurchaseOrder_850.DTM();
            dateRequested.DateTimeQualifier = "126";
            dateRequested.Date = DateTime.Parse("2024/09/16");

            List<PurchaseOrder_850.OrderDetail> items = new List<PurchaseOrder_850.OrderDetail>();
            items.Add(lineItem);

            var gsGroup = new PurchaseOrder_850.FunctionalGroup();
            gsGroup.Date = DateTime.Now;
            gsGroup.ApplicationSenderCode = "Sender";
            gsGroup.ApplicaitonReceiverCode = "Receiver";
            gsGroup.FunctionalIdentifierCode = "PO";
            gsGroup.GroupControlNumber = 9001;
            gsGroup.TransactionsCount = 1;
            gsGroup.Version = "004010";
            gsGroup.AgencyCode = "X";

            List<PurchaseOrder_850.Order> orders =
                new List<PurchaseOrder_850.Order>();
            orders.Add(order);

            po850.AcknowledgementRequested = "N";
            po850.Component_Element_Seperator = '*';
            po850.ControlNumber = 20001;
            po850.ControlVersion = 004010;
            po850.Control_Standards_ID = "U";
            po850.ID_Qualifier = "ZZ";
            po850.ID_Qualifier2 = "ZZ";
            po850.Sender_ID = "WALMART";
            po850.Receiver_ID = "MEGACORP";
            po850.Usage_Indicator = "P";

            List<PurchaseOrder_850.FunctionalGroup> groups = new List<PurchaseOrder_850.FunctionalGroup>();
            groups.Add(group);
            return po850;
        }

        public static void serializePO850(PurchaseOrder_850 argPO850, EdiGrammar ediGrammar, IEdiGrammar ediGrammar1) {


            //string inputEDIFilename = @"c:\EDIClass\Downloads\EDI.Net-master\?";
            string outputEDIFilename = @"c:\EDIClass\Samples\Sample_EDINET_Serialize01.edi";

            //serialize to file.
            using (var textWriter = new StreamWriter(File.OpenWrite(outputEDIFilename))) {
                var grammar = ediGrammar1;


                grammar.SetAdvice(
                segmentNameDelimiter: '*',
                dataElementSeparator: '*',
                componentDataElementSeparator: '>',
                segmentTerminator: '~',
                releaseCharacter: null,
                reserved: null,
                decimalMark: '.');

                // serialize to file.
                StreamWriter streamWriter = new StreamWriter(File.Open(@"c:\temp\out.edi", FileMode.Create));
                using (var textWriter = streamWriter) {
                    using (var ediWriter = new EdiTextWriter(textWriter, grammar)) {
                        new EdiSerializer().Serialize(ediWriter, argPO850);
                    }

                }
            }
        }
    }
}
