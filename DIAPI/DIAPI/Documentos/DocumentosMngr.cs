using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI.Documentos
{
    public class DocumentosMngr
    {
        private Company _Company;
        public DocumentosMngr(Company company)
        {
            this._Company = company;
        }

        public int CrearPedido()
        {

            try
            {
                Documents pedidoSAP = _Company.GetBusinessObject(BoObjectTypes.oOrders);

                pedidoSAP.CardCode = "CL20100018625";
                pedidoSAP.TaxDate = DateTime.Now;
                pedidoSAP.DocDueDate = DateTime.Now;
                pedidoSAP.DocDate = DateTime.Now;

                pedidoSAP.Comments = "CREADO DESDE DI API CONTINENTAL";
                pedidoSAP.JournalMemo = "Comentario de asiento pedido";

                pedidoSAP.Lines.ItemCode = "1184.1101.13488.15745";
                pedidoSAP.Lines.Quantity = 20;
                pedidoSAP.Lines.UnitPrice = 50.5;
                pedidoSAP.Lines.WarehouseCode = "1001";
                pedidoSAP.Lines.TaxCode = "I18";

                pedidoSAP.Lines.Add();

                pedidoSAP.Lines.ItemCode = "1184.1101.13488.15745";
                pedidoSAP.Lines.Quantity = 30;
                pedidoSAP.Lines.UnitPrice = 20.5;
                pedidoSAP.Lines.WarehouseCode = "1001";
                pedidoSAP.Lines.TaxCode = "I18";

                pedidoSAP.Lines.Add();


                if (pedidoSAP.Add() != 0)
                    throw new Exception(_Company.GetLastErrorDescription());

                return Convert.ToInt32(_Company.GetNewObjectKey());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int CrearEntrega(int idPedido)
        {
            try
            {
                Documents entregaSAP = _Company.GetBusinessObject(BoObjectTypes.oDeliveryNotes);

                entregaSAP.CardCode = "CL20100018625";
                entregaSAP.TaxDate = DateTime.Now;
                entregaSAP.DocDueDate = DateTime.Now;
                entregaSAP.DocDate = DateTime.Now;

                entregaSAP.Comments = "CREADO DESDE DI API CONTINENTAL";
                entregaSAP.JournalMemo = "Comentario de asiento entrega";

                entregaSAP.Lines.Quantity = 10;
                entregaSAP.Lines.BaseEntry = idPedido;
                entregaSAP.Lines.BaseLine = 0;
                entregaSAP.Lines.BaseType = 17;

                entregaSAP.Lines.BatchNumbers.BatchNumber = "2212000078B00010";
                entregaSAP.Lines.BatchNumbers.Quantity = 10;
                entregaSAP.Lines.BatchNumbers.Add();

                entregaSAP.Lines.Add();

                entregaSAP.Lines.Quantity = 15;
                entregaSAP.Lines.BaseEntry = idPedido;
                entregaSAP.Lines.BaseLine = 1;
                entregaSAP.Lines.BaseType = 17;

                entregaSAP.Lines.BatchNumbers.BatchNumber = "2212000078B00010";
                entregaSAP.Lines.BatchNumbers.Quantity = 15;
                entregaSAP.Lines.BatchNumbers.Add();

                entregaSAP.Lines.Add();


                if (entregaSAP.Add() != 0)
                    throw new Exception(_Company.GetLastErrorDescription());

                return Convert.ToInt32(_Company.GetNewObjectKey());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int CrearFactura(int idEntrega)
        {

            try
            {
                Documents facturaSAP = _Company.GetBusinessObject(BoObjectTypes.oInvoices);

                //CREANDO FACTURA CON REFERENCIA A UNA ENTREGA COMPLETA
                Documents entregaSAP = _Company.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                entregaSAP.GetByKey(idEntrega);

                facturaSAP.CardCode = entregaSAP.CardCode;
                facturaSAP.Comments = "Factura desde DI API";
                facturaSAP.Series = 84;
                facturaSAP.NumAtCard = "01F001-00003433";
                facturaSAP.TaxDate = entregaSAP.TaxDate;
                facturaSAP.DocDate = entregaSAP.DocDate;
                facturaSAP.DocDueDate = entregaSAP.DocDueDate;

                for (int i = 0; i < entregaSAP.Lines.Count; i++)
                {
                    entregaSAP.Lines.SetCurrentLine(i); //PERMITE UBICARTE EN LA LINEA i DE LA ENTREGA

                    facturaSAP.Lines.Add();
                    facturaSAP.Lines.ItemCode = entregaSAP.Lines.ItemCode;
                    facturaSAP.Lines.Quantity = entregaSAP.Lines.Quantity;
                    facturaSAP.Lines.UnitPrice = entregaSAP.Lines.UnitPrice;
                    facturaSAP.Lines.BaseEntry = idEntrega;
                    facturaSAP.Lines.BaseLine = entregaSAP.Lines.LineNum;
                    facturaSAP.Lines.BaseType = 15;
                }


                if (facturaSAP.Add() != 0)
                    throw new Exception(_Company.GetLastErrorDescription());

                return Convert.ToInt32(_Company.GetNewObjectKey());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int CrearPago(int idFactura)
        {

            try
            {
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
