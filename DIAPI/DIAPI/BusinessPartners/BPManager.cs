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

        public void Crear() //
        { 
        
        }

        public void Eliminar(string codigo)
        { 
            
        }

        public void Actualizar(string codigo)
        { 
        
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
