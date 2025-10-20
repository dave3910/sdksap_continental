using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI.Connection
{
    public class ConexionSAP
    {
        public string Server { get; set; }
        public string DBName { get; set; }
        public string DBUserName { get; set; }
        public string DBPass { get; set; }
        public string SAPUser { get; set; }
        public string SAPPass { get; set; }

        private Company _company;

        public ConexionSAP(string server, string dbName,string dbUserName, string dbPass, string sapUser, string sapPass)
        {
            this.Server = server;
            this.DBName = dbName;
            this.DBPass = dbPass;
            this.SAPUser = sapUser;
            this.SAPPass = sapPass;
            this.DBUserName = dbUserName;

        }

        public Company GetSAPConnection()
        {
            try
            {
                _company = new Company();
                _company.Server = this.Server;
                _company.CompanyDB = this.DBName;
                _company.DbUserName = this.DBUserName;
                _company.DbPassword = this.DBPass;
                _company.UserName = this.SAPUser;
                _company.Password = this.SAPPass;

                _company.DbServerType = BoDataServerTypes.dst_HANADB;
                _company.language = BoSuppLangs.ln_Spanish_La;

                int ret = _company.Connect();
                if (ret != 0)
                    throw new Exception(_company.GetLastErrorDescription());

                return _company;
                
            }
            catch (Exception)
            {
                throw ;
            }
        }

    }
}
