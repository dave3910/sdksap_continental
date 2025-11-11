
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Xml;

namespace SBOAddonContinental
{
    [FormAttribute("SBOAddonContinental.Form1", "Form1.b1f")]
    class Form1 : UserFormBase
    {
        public Form1()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_2").Specific));
            this.TxtNombreCliente = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
            this.TxtNombreCliente.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.TxtNombreCliente_ChooseFromListAfter);
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_4").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_5").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.EditText TxtNombreCliente;

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                //EVENTO CONSULTAR

                //1. LEER EL CODIGO DE SOCIO

                string codigoCliente = TxtNombreCliente.Value;
                if (string.IsNullOrEmpty(codigoCliente))
                    throw new Exception("Debe seleccionar código del cliente");

                //2. CONSULTAR A BASE DE DATOS

                SAPbouiCOM.DataTable dt = UIAPIRawForm.DataSources.DataTables.Item("DT_INV");
                string query = $"SELECT TOP 10 \"DocEntry\", \"DocNum\", \"NumAtCard\" FROM OINV WHERE \"CardCode\" = '{codigoCliente}' ";
                //loggear el query

                //3. MOSTRAR INFORMACIÓN EN LA GRILLA
                dt.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                Console.WriteLine(ex.Message); //LOGGEAR Y MOSTRAR el error
            }

        }

        private void TxtNombreCliente_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            //SAPbouiCOM.ItemEvent itmEvent = (SAPbouiCOM.ItemEvent)pVal;

            SAPbouiCOM.SBOChooseFromListEventArg oCFLEvento = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;

            if(pVal.ActionSuccess)
            {
                if (oCFLEvento.SelectedObjects is SAPbouiCOM.DataTable dtbl)
                {
                    try
                    {
                        ((SAPbouiCOM.EditText)UIAPIRawForm.Items.Item("Item_3").Specific).Value = dtbl.GetValue("CardCode", 0).ToString();


                    }
                    catch (Exception)
                    { }


                    try
                    {
                        ((SAPbouiCOM.EditText)UIAPIRawForm.Items.Item("Item_5").Specific).Value = dtbl.GetValue("CardName", 0).ToString();

                    }
                    catch (Exception)
                    { }
                }

            }
        }

        private SAPbouiCOM.Grid Grid0;
        private SAPbouiCOM.EditText EditText1;
    }
}