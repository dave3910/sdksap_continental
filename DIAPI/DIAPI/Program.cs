using DIAPI.BusinessPartners;
using DIAPI.Connection;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //SERVIDOR SAP (IP O HOSTNAME + PUERTO)
            //TIPO DE BASE DE DATOS (SQL / HANA)
            //EL NOMBRE DEL ESQUEMA DE ACCESO
            //USUARIO DE BASE DE DATOS
            //PASSWORD DE BASE DE DATOS
            //USUARIO SAP
            //PASSWORD SAP
            //LANGUAJE

            //OBTENEMOS CONEXIÓN
            
            string server = "NDB@192.168.1.14:30013";
            string schema = "B1H_LAZZOS_PROD2503";
            string userdb = "SYSTEM";
            string passdb = "HANAB1Admin";
            string usersap = "manager";
            string passsap = "sbosap";

            Company company = null;

            try
            {
                ConexionSAP con = new ConexionSAP(server, schema, userdb, passdb, usersap, passsap);
                company = con.GetSAPConnection();

                Console.WriteLine($"Conexión establecida correctamente con esquema {company.CompanyName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //DATOS MAESTROS
            //CLIENTES (SOCIOS DE NEGOCIO)

            BPManager bpManager = new BPManager(company);
            bpManager.Existe("PL20100686814");

            SAPbobsCOM.BusinessPartners bp = bpManager.Get("PL20100686814");
            Console.WriteLine($"Código: {bp.CardCode} - Rázón social: {bp.CardName} - RUC: {bp.FederalTaxID}");


            try
            {

                Console.WriteLine("Iniciando consulta de socios filtrados por query");
                List<BusinessPartner> listaFiltrada = bpManager.GetListaDesdeQuery("C"); //CLIENTES = C , PROVEEDORES = S

                foreach (var socio in listaFiltrada)
                {
                    Console.WriteLine($"Socio: {socio.CodigoSocio} - Razon Social: {socio.RazonSocial} - RUC: {socio.RUC}");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }



            //ARTICULOS
            //

            Console.ReadKey();


        }
    }
}
