using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI.BusinessPartners
{
    public class BPManager
    {
        private Company _SAPConn;
        private SAPbobsCOM.BusinessPartners _bp;

        public BPManager(Company company)
        {
            _SAPConn = company;
            _bp = company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
        }

        public SAPbobsCOM.BusinessPartners Get(string codigo) 
        {
            if (!Existe(codigo))
                throw new Exception($"El socio de negocio con código {codigo}, no se encuentra en el sistema");

            return _bp;
        }

        public bool Existe(string codigo)
        {  
            return _bp.GetByKey(codigo);
        }

        public bool Crear(BusinessPartner obp) //
        {
            try
            {
                //LLENAR LOS DATOS DEL SOCIO
                _bp = _SAPConn.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                _bp.CardCode = obp.CodigoSocio;
                _bp.CardName = obp.RazonSocial;
                _bp.FederalTaxID = obp.RUC;

                //CAMPOS DE USUARIO
                _bp.UserFields.Fields.Item("U_CL_CAMPOUSUARIO").Value = "TPN";


                //COLECCIONES DEPENDIENTES (DIRECCIONES, PERSONAS DE CONTACTO)
                _bp.Addresses.AddressName = "FISCAL";
                _bp.Addresses.AddressType = BoAddressType.bo_BillTo;
                _bp.Addresses.Street = "CALLE RENE DESCARTES 114 - ATE - LIMA"; //direccion detallada
                _bp.Addresses.County = "ATE";
                _bp.Addresses.State = "15";
                _bp.Addresses.City = "LIMA";
                _bp.Addresses.GlobalLocationNumber = "150326";//UBIGEO

                _bp.Addresses.Add();


                _bp.Addresses.AddressName = "ALMACEN";
                _bp.Addresses.AddressType = BoAddressType.bo_ShipTo;
                _bp.Addresses.Street = "CALLE RENE DESCARTES 114 - ATE - LIMA"; //direccion detallada
                _bp.Addresses.County = "ATE";
                _bp.Addresses.State = "15";
                _bp.Addresses.City = "LIMA";
                _bp.Addresses.GlobalLocationNumber = "150326";//UBIGEO

                _bp.Addresses.Add();

                int rpta = _bp.Add();
                if (rpta == 0)
                    return true;

                throw new Exception(_SAPConn.GetLastErrorDescription());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Eliminar(string codigo)
        {
            try
            {
                if (!_bp.GetByKey(codigo))
                    throw new Exception($"El socio no existe {codigo}");

                if (_bp.Remove() != 0)
                    throw new Exception(_SAPConn.GetLastErrorDescription());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Actualizar(string codigo)
        {
            try
            {
                if (!_bp.GetByKey(codigo))
                    throw new Exception($"El socio no existe {codigo}");

                _bp.CardName = _bp.CardName + "-ACTUALIZADO";

                if (_bp.Update() != 0)
                    throw new Exception(_SAPConn.GetLastErrorDescription());
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal List<BusinessPartner> GetListaDesdeQuery(string tipoSocio)
        {
            BusinessPartner mybp;
            List<BusinessPartner> myList = new List<BusinessPartner>();

            try
            {
                Recordset rs = _SAPConn.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"SELECT TOP 10 \"CardCode\" AS  \"CodigoSocio\", \"CardName\", \"LicTradNum\" FROM OCRD WHERE \"CardType\" = '{tipoSocio}'";
                rs.DoQuery(query);

                if (rs.RecordCount == 0)
                    throw new Exception("No se han encontrado registros con los filtros seleccionados");

                while (!rs.EoF)
                {
                    myList.Add(new BusinessPartner() { 
                        CodigoSocio = rs.Fields.Item("CodigoSocio").Value,
                        RazonSocial = rs.Fields.Item("CardName").Value,
                        RUC = rs.Fields.Item("LicTradNum").Value
                    });

                    rs.MoveNext();
                }

                return myList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
