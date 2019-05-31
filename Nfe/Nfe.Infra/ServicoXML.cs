using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;
using Nfe.Infra.AcessoDados;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;

namespace Nfe.Infra
{
    public class ServicoXML
    {
        private NotaFiscal Nota { get; set; }
        public string PathDestino { get; set; }
        public string PathRetornoLote { get; set; }
        public string PathRetornoSituacaoErr { get; set; }

        public string NumLoteGerado { get; set; }
        public string NumRecibo { get; set; }
        public string NumProtocolo { get; set; }
        public string Resposta { get; set; }


        public bool GerouNumeroLoteOK { get; set; }
        public bool GerouRespostaLoteOK { get; set; }
        public bool GerouNumeroReciboOK { get; set; }
        public bool GerouProtocoloOK { get; set; }

        private XmlDocument doc { get; set; }
        public ConfiguracaoApp config { get; set; }
     
        public ServicoXML(NotaFiscal nota)
        {
            this.Nota = nota;
            doc = new XmlDocument();
        }
        public void geraNotaEmXML()
        {
            
            XmlElement infNfe = doc.CreateElement("infNFe");
            infNfe.SetAttribute("Id", "NFe" + Nota.IDChave);
            infNfe.SetAttribute("versao", "2.00");

            XmlElement Nfe = doc.CreateElement("NFe");
            Nfe.SetAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe");
            Nfe.AppendChild(infNfe);
            
            infNfe.AppendChild(Nota.toNfeXml(doc));
            infNfe.AppendChild(Nota.Emitente.EmitToNfeXml(doc));
            infNfe.AppendChild(Nota.Cliente.toNfeXml(doc,Nota.tpAmb));

            foreach (var item in getProdutosToXml())
            {
                infNfe.AppendChild(item);
            }
            foreach (var item in getTotalToXml(Nota))
            {
                infNfe.AppendChild(item);
            }
            foreach (var item in getTransporteToXml())
                {
                    infNfe.AppendChild(item);
                }
            foreach (var item in getCobrancaToXml())
            {
                infNfe.AppendChild(item);
            }
            config = new ConfiguracaoApp();

            this.PathDestino = config.PastaXmlEnvio + "\\" + Nota.IDChave + "-nfe.xml";
            this.PathRetornoLote = config.PastaXmlRetorno + "\\" + Nota.IDChave + "-num-lot.xml";
            this.PathRetornoSituacaoErr = config.PastaXmlRetorno + "\\" + Nota.IDChave + "-sit.err";
            XmlElement root = doc.DocumentElement;
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
                
            doc.InsertBefore(xmlDec, root);
            doc.AppendChild(Nfe);
            doc.Save(this.PathDestino);
            Thread.Sleep(2000);
            
            //Assinar();
            //Validar();
            if (Nota.tpEmis == 1)
            {
                GetLoteDaNota();
                AtualizaNota();
            }
            else if(Nota.tpEmis == 2)
            {
                this.Resposta += "Nota gerada em contingencia FS";
            }
            
        }
        private List<XmlElement> getProdutosToXml()
        {
            List<XmlElement> lista = new List<XmlElement>();
            int i = 1;
            foreach (var item in Nota.Produtos)
            {

                XmlElement det = doc.CreateElement("det");
                det.SetAttribute("nItem", i.ToString());
                XmlElement prod = doc.CreateElement("prod");
                XmlElement cProd = doc.CreateElement("cProd"); cProd.InnerText = item.cProd.ToString().Trim();
                XmlElement cEAN = doc.CreateElement("cEAN");                
                XmlElement xProd = doc.CreateElement("xProd"); xProd.InnerText = item.xProd.Trim();
                XmlElement NCM = doc.CreateElement("NCM"); NCM.InnerText = item.NCM;
                XmlElement CFOP = doc.CreateElement("CFOP"); CFOP.InnerText = item.CFOP.ToString().Trim();
                XmlElement uCom = doc.CreateElement("uCom"); uCom.InnerText = item.uCom;
                XmlElement qCom = doc.CreateElement("qCom"); qCom.InnerText = item.qCom.ToString("#0.0000").Replace(',', '.');
                XmlElement vUnCom = doc.CreateElement("vUnCom"); vUnCom.InnerText = item.vUnCom.ToString("#0.0000000").Replace(',', '.');
                XmlElement vProd = doc.CreateElement("vProd"); vProd.InnerText = item.vProd.ToString("#0.00").Replace(',', '.');
                XmlElement cEANTrib = doc.CreateElement("cEANTrib");
                XmlElement uTrib = doc.CreateElement("uTrib"); uTrib.InnerText = item.uTrib;
                XmlElement qTrib = doc.CreateElement("qTrib"); qTrib.InnerText = item.qTrib.ToString("#0.0000").Replace(',', '.');
                XmlElement vUnTrib = doc.CreateElement("vUnTrib"); vUnTrib.InnerText = item.vUnTrib.ToString("#0.0000000").Replace(',', '.');
                
                    XmlElement vFrete = doc.CreateElement("vFrete"); vFrete.InnerText = item.vFrete.ToString("#0.00").Replace(',', '.');
                
                XmlElement indTot = doc.CreateElement("indTot"); indTot.InnerText = item.indTot.ToString();
                
                //XmlElement xPed = doc.CreateElement("xPed"); xPed.InnerText = item.x
                prod.AppendChild(cProd);
                prod.AppendChild(cEAN);                               
                prod.AppendChild(xProd);

                //Modificado a pedido do Hatori dia 27/07/2011
                //prod.AppendChild(NCM);
                if (NCM.InnerText.Length > 1)
                {
                    prod.AppendChild(NCM);
                }

                prod.AppendChild(CFOP);
                prod.AppendChild(uCom);
                prod.AppendChild(qCom);
                prod.AppendChild(vUnCom);
                prod.AppendChild(vProd);
                prod.AppendChild(cEANTrib);
                prod.AppendChild(uTrib);
                prod.AppendChild(qTrib);
                prod.AppendChild(vUnTrib);
                if (vFrete.InnerText != "0.00")
                {
                    prod.AppendChild(vFrete);
                }
                prod.AppendChild(indTot);
                
               // prod.AppendChild(xPed);
                
                XmlElement imposto = doc.CreateElement("imposto");
                
                if ((item.SimpNac == null ? 0 : item.SimpNac.Count) > 0)
                    {
                        try
                        {
                            foreach (var simples in item.SimpNac)
                            {
                                imposto.AppendChild(simples.toNfeXml(doc));
                            }
                        }
                    
                        catch (Exception ex)
                        {

                            throw new Exception("Favor verificar impostos simples, " + ex.Message);
                        }
                    
                    }
                if ((item.PIS == null ? 0 : item.PIS.Count) > 0)
                {
                     try
                    {
                        foreach (var pis in item.PIS)
                        {
                            imposto.AppendChild(pis.toNfeXml(doc));
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Favor verificar impostos PIS, " +ex.Message);
                    }
                    
                }
                if ((item.PISST == null ? 0 : item.PISST.Count) > 0)
                {
                    try
                    {
                        foreach (var pisst in item.PISST)
                        {
                            //if ((pisst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.pPIS.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.vPIS.Value.ToString("#0.00").Replace(",", ".") != "0.00") || (pisst.vAliqProd.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.qBCProd.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.vPIS.Value.ToString("#0.00").Replace(",", ".") != "0.00"))
                            if ((pisst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.pPIS.Value.ToString("#0.00").Replace(",", ".") != "0.00") || 
                                (pisst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.vPIS.Value.ToString("#0.00").Replace(",", ".") != "0.00") ||
                                (pisst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.vAliqProd.Value.ToString("#0.00").Replace(",", ".") != "0.00") ||
                                (pisst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && pisst.qBCProd.Value.ToString("#0.00").Replace(",", ".") != "0.00"))
                            {
                                imposto.AppendChild(pisst.toNfeXml(doc));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Favor verificar impostos PISST, " + ex.Message);
                    }
                    
                }
                if ((item.COFINS == null ? 0 : item.COFINS.Count) > 0)
                {
                    try
                    {
                        foreach (var cofins in item.COFINS)
                        {
                            imposto.AppendChild(cofins.toNfeXml(doc));
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Favor verificar impostos COFINS, " + ex.Message);
                    }
                }
                if ((item.COFINSST == null ? 0 : item.COFINSST.Count) > 0)
                {
                    try
                    {
                        foreach (var cofinsst in item.COFINSST)
                        {
                            //if ((cofinsst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.pCOFINS.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.vCOFINS.Value.ToString("#0.00").Replace(",", ".") != "0.00") || (cofinsst.vAliqProd.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.qBCProd.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.vCOFINS.Value.ToString("#0.00").Replace(",", ".") != "0.00"))
                            if ((cofinsst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.pCOFINS.Value.ToString("#0.00").Replace(",", ".") != "0.00") ||
                                (cofinsst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.vCOFINS.Value.ToString("#0.00").Replace(",", ".") != "0.00") ||
                                (cofinsst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.vAliqProd.Value.ToString("#0.00").Replace(",", ".") != "0.00") ||
                                (cofinsst.vBC.Value.ToString("#0.00").Replace(",", ".") != "0.00" && cofinsst.qBCProd.Value.ToString("#0.00").Replace(",", ".") != "0.00"))
                            {
                                imposto.AppendChild(cofinsst.toNfeXml(doc));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Favor verificar impostos COFINSST, " + ex.Message);
                    }
                    
                }
                det.AppendChild(prod);
                det.AppendChild(imposto);
                lista.Add(det);
                i++;
            }
            return lista;
        }
        private List<XmlElement> getTotalToXml( NotaFiscal nota)
        {
            decimal vTotPIS = 0;
            decimal vTotCOFINS = 0;
            
            foreach (var item in Nota.Produtos)
            {
                vTotPIS += item.PIS[0].vPIS.Value; vTotCOFINS += item.COFINS[0].vCOFINS.Value;
            }
            
            List<XmlElement> lista = new List<XmlElement>();

            XmlElement total = doc.CreateElement("total");
            XmlElement ICMSTot = doc.CreateElement("ICMSTot");
            XmlElement vBC = doc.CreateElement("vBC"); vBC.InnerText = nota.vBC.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vICMS = doc.CreateElement("vICMS"); vICMS.InnerText = nota.vICMS.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vBCST = doc.CreateElement("vBCST"); vBCST.InnerText = nota.vBCST.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vST = doc.CreateElement("vST"); vST.InnerText = nota.vST.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vProd = doc.CreateElement("vProd"); vProd.InnerText = Nota.vProd.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vFrete = doc.CreateElement("vFrete"); vFrete.InnerText = Nota.vFrete.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vSeg = doc.CreateElement("vSeg"); vSeg.InnerText = nota.vSeg.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vDesc = doc.CreateElement("vDesc"); vDesc.InnerText = nota.vDesc.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vII = doc.CreateElement("vII"); vII.InnerText = nota.vII.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vIPI = doc.CreateElement("vIPI"); vIPI.InnerText = nota.vIPI.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vPIS = doc.CreateElement("vPIS"); vPIS.InnerText = vTotPIS.ToString("#0.00").Replace(',','.');
            XmlElement vCOFINS = doc.CreateElement("vCOFINS"); vCOFINS.InnerText = vTotCOFINS.ToString("#0.00").Replace(',','.');
            XmlElement vOutro = doc.CreateElement("vOutro"); vOutro.InnerText = nota.vOutro.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vNF = doc.CreateElement("vNF"); vNF.InnerText = nota.vNF.Value.ToString("#0.00").Replace(',', '.');
            ICMSTot.AppendChild(vBC);
            ICMSTot.AppendChild(vICMS);
            ICMSTot.AppendChild(vBCST);
            ICMSTot.AppendChild(vST);
            ICMSTot.AppendChild(vProd);
            ICMSTot.AppendChild(vFrete);
            ICMSTot.AppendChild(vSeg);
            ICMSTot.AppendChild(vDesc);
            ICMSTot.AppendChild(vII);
            ICMSTot.AppendChild(vIPI);
            ICMSTot.AppendChild(vPIS);
            ICMSTot.AppendChild(vCOFINS);
            ICMSTot.AppendChild(vOutro);
            ICMSTot.AppendChild(vNF);

            total.AppendChild(ICMSTot);
            lista.Add(total);
            return lista;
        }
        private List<XmlElement> getTransporteToXml()
        {
            List<XmlElement> lista = new List<XmlElement>();
            XmlElement transp = doc.CreateElement("transp");
            XmlElement modFrete = doc.CreateElement("modFrete"); modFrete.InnerText = "1";

            transp.AppendChild(modFrete);

            if (Nota.Transportadora != null)
            {

                XmlElement transporta = doc.CreateElement("transporta");
                XmlElement CNPJ = doc.CreateElement("CNPJ"); CNPJ.InnerText = Nota.Transportadora.DadosEmpresa.Cnpj.Trim();
                XmlElement xNome = doc.CreateElement("xNome"); xNome.InnerText = Nota.Transportadora.DadosEmpresa.NomeOURazao.Trim();
                XmlElement IE = doc.CreateElement("IE"); IE.InnerText = Nota.Transportadora.DadosEmpresa.IE.Trim();
                XmlElement xEnder = doc.CreateElement("xEnder"); xEnder.InnerText = Nota.Transportadora.DadosEmpresa.Endereco.Logradouro.Trim();
                XmlElement xMun = doc.CreateElement("xMun"); xMun.InnerText = Nota.Transportadora.DadosEmpresa.Endereco.Municipio.Trim();
                XmlElement UF = doc.CreateElement("UF"); UF.InnerText = Nota.Transportadora.DadosEmpresa.Endereco.UF.Trim();

                transporta.AppendChild(CNPJ);
                transporta.AppendChild(xNome);
                transporta.AppendChild(IE);
                transporta.AppendChild(xEnder);
                transporta.AppendChild(xMun);
                transporta.AppendChild(UF);

                transp.AppendChild(transporta);
            }

            XmlElement vol = doc.CreateElement("vol");
            XmlElement qVol = doc.CreateElement("qVol"); qVol.InnerText = Nota.volume.Value.ToString("#0");
            XmlElement esp = doc.CreateElement("esp"); esp.InnerText = Nota.esp.Trim();
            XmlElement marca = doc.CreateElement("marca"); marca.InnerText = Nota.marca.Trim();
            XmlElement nVol = doc.CreateElement("nVol"); nVol.InnerText = Nota.volume.ToString().Trim();
            XmlElement pesoL = doc.CreateElement("pesoL"); pesoL.InnerText = Nota.pesoLiq.Trim().Replace(',', '.');
            XmlElement pesoB = doc.CreateElement("pesoB"); pesoB.InnerText = Nota.pesoBruto.Trim().Replace(',', '.');
            
            if (Nota.volume.Value.ToString("#0") != "0")
            {
                vol.AppendChild(qVol);
            }
            if (Nota.esp.Trim() != "")
            {
                vol.AppendChild(esp);
            }
            if (Nota.marca.Trim() != "")
            {
                vol.AppendChild(marca);
            }
            
            //if (Nota.pesoLiq.Trim().Replace(',', '.') != "0.000")
            if ((Nota.pesoLiq == "" ? 0 : Convert.ToDouble(Nota.pesoLiq)) != 0)
            {
                vol.AppendChild(pesoL);
            }
                //vol.AppendChild(nVol);
            //if (Nota.pesoBruto.Trim().Replace(',', '.') != "0.000")
            if ((Nota.pesoBruto == "" ? 0 : Convert.ToDouble(Nota.pesoBruto)) != 0)
            {
                vol.AppendChild(pesoB);
            }
            //Criado por conta que agora ele não permite a tag vol em branco. Ou existe ou não existe.
            //Cristoffer 11/10/2012
            if ((Nota.volume.Value.ToString("#0") != "0") || (Nota.esp.Trim() != "") || (Nota.marca.Trim() != "") || ((Nota.pesoLiq == "" ? 0 : Convert.ToDouble(Nota.pesoLiq)) != 0))
            {
                transp.AppendChild(vol);
            }

            lista.Add(transp);
            return lista;
        }
        private List<XmlElement> getCobrancaToXml()
        {
            List<XmlElement> lista = new List<XmlElement>();

            XmlElement cobr = doc.CreateElement("cobr");
            XmlElement fat = doc.CreateElement("fat");
            XmlElement nFat = doc.CreateElement("nFat"); nFat.InnerText = Nota.indPag.ToString(); 
            string vnf=Nota.vNF.Value.ToString("#0.00").Replace(',', '.');
            XmlElement vOrig = doc.CreateElement("vOrig"); vOrig.InnerText = vnf;
            XmlElement vLiq = doc.CreateElement("vLiq"); vLiq.InnerText = vnf;


            fat.AppendChild(nFat);
            fat.AppendChild(vOrig);
            fat.AppendChild(vLiq);
            cobr.AppendChild(fat);
            lista.Add(cobr);
            return lista;
        }
        private string getDateNow()
        {
            var dt = DateTime.Now;
            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();
        }
        private void Assinar()
        {
            AssinaturaDigital ass = new AssinaturaDigital();
            CertificadoDigital cert = new CertificadoDigital();
            ConfiguracaoApp config = new ConfiguracaoApp();
            ass.Assinar(doc, this.PathDestino, "infNFe", config.X509Certificado);
        }
        private void Validar()
        {
            ValidarXMLs oValidarXML = new ValidarXMLs();
            oValidarXML.Validar(PathDestino, "nfe_v2.00.xsd");
            if (oValidarXML.Retorno == 0)
            {
                throw new Exception("0-Validado com sucesso");
            }
            else
            {
                throw new Exception(oValidarXML.RetornoString);
            }
        }

        public void GetLoteDaNota()
        {
            //TROCA ARQUIVO
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(2000);
                if (GetNumeroLote())
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            if (this.GerouNumeroLoteOK)
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(2000);
                    if (getRetornoDoLote())
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }                
            }
            if (this.GerouNumeroReciboOK)
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(2000);
                    if (getRetornoDoRecibo())
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }    
            }
        }

        private bool getRetornoDoRecibo()
        {
            bool retorno = false;
            this.GerouProtocoloOK = false;
            string PathRetornoXML = config.PastaXmlRetorno + "\\" + this.NumRecibo + "-pro-rec.xml";
            string PathRetornoXMLErr = config.PastaXmlRetorno + "\\" + this.NumRecibo + "-pro-rec.err";
            FileInfo arq = new FileInfo(PathRetornoXML);
            FileInfo arqErr = new FileInfo(PathRetornoXMLErr);
            if (arq.Exists)
            {
              
                XmlDocument xml = new XmlDocument();
                xml.Load(PathRetornoXML);
                XmlNodeList retConsStatServ = null;
                retConsStatServ = xml.GetElementsByTagName("retConsReciNFe");
                foreach (XmlNode consStatServNode in retConsStatServ)
                {
                     XmlNodeList infRecList = xml.GetElementsByTagName("protNFe");
                     foreach (XmlNode infRecNode in infRecList)
                     {
                         XmlNodeList infProtList = xml.GetElementsByTagName("infProt");
                         foreach (XmlNode infProtNode in infProtList)
                         {
                             XmlElement infRecElemento = (XmlElement)infProtNode;

                             //this.Resposta = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                             this.Resposta = new MensagemRetorno().MensagemDeRetorno(Convert.ToInt32(infRecElemento.GetElementsByTagName("cStat")[0].InnerText),infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText);
                             try
                             {
                                 this.NumProtocolo = infRecElemento.GetElementsByTagName("nProt")[0].InnerText;
                                 this.GerouProtocoloOK = true;
                                 retorno = true;
                             }
                             catch(Exception ex)
                             {
                                 this.GerouProtocoloOK = false;
                                 throw new Exception(infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText);
                             }
                             
                             
                         }                         
                     }                    
                    
                }
            }
            else if (arqErr.Exists)
            {
                using (StreamReader sr = arqErr.OpenText())
                {
                    string s = "";
                    string resp = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        resp += s;
                        retorno = true;
                    }
                    UTF8Encoding utf88 = new UTF8Encoding();
                    byte[] byteArray = Encoding.ASCII.GetBytes(resp);
                    byte[] utf8Array = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, byteArray);
                    string finalString = utf88.GetString(utf8Array);
                    this.Resposta = finalString;
                }
            }
            else
            {
                retorno = false;
                this.Resposta = "Não foi possivel verificar sistema sefaz, tente novamente mais tarde ou entre em contato com o suporte";
            }
            return retorno;   
        }        
        public bool getRetornoDoLote()
        {
            bool retorno = false;
            string arqLote = this.config.PastaXmlRetorno + "\\" + this.NumLoteGerado.PadLeft(15, '0') + "-rec.xml";
            string arqLoteErr = this.config.PastaXmlRetorno + "\\" + this.NumLoteGerado.PadLeft(15, '0') + "-rec.err";

            FileInfo arqLt = new FileInfo(arqLote);
            FileInfo arqLtErr = new FileInfo(arqLoteErr);

            if (arqLt.Exists)
            {

                //VERIFICAR QUE TIPO DE ERRO ELE RETORNA
                XmlDocument xml = new XmlDocument();
                xml.Load(arqLote);
                XmlNodeList retEnviNFeList = null;
                retEnviNFeList = xml.GetElementsByTagName("retEnviNFe");
                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                    //this.oDadosRec.cStat = retEnviNFeElemento.GetElementsByTagName("cStat")[0].InnerText;

                    XmlNodeList infRecList = xml.GetElementsByTagName("infRec");

                    foreach (XmlNode infRecNode in infRecList)
                    {
                        XmlElement infRecElemento = (XmlElement)infRecNode;

                        this.NumRecibo = infRecElemento.GetElementsByTagName("nRec")[0].InnerText;
                        //this.oDadosRec.tMed = Convert.ToInt32(infRecElemento.GetElementsByTagName("tMed")[0].InnerText);
                        this.GerouNumeroReciboOK = true;
                        retorno = true;
                    }
                }
            }
            else if (arqLtErr.Exists)
            {
                using (StreamReader sr = arqLtErr.OpenText())
                {
                    string s = "";
                    string resp = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        resp += s;                        
                    }
                    retorno = true;
                    UTF8Encoding utf88 = new UTF8Encoding();
                    byte[] byteArray = Encoding.ASCII.GetBytes(resp);
                    byte[] utf8Array = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, byteArray);
                    string finalString = utf88.GetString(utf8Array);
                    this.Resposta = finalString;
                    this.GerouNumeroReciboOK = false;
                }
            }
            else
            {
                retorno = false;
                this.GerouNumeroReciboOK = false;
                this.Resposta = "Não foi possivel verificar sistema sefaz, tente novamente mais tarde ou entre em contato com o suporte";
            }
            return retorno;
        }
        private bool GetNumeroLote()
        {
            bool retorno = false;
            this.GerouNumeroLoteOK = false;
            FileInfo arqRetLote = new FileInfo(this.PathRetornoLote);
            FileInfo arqErr = new FileInfo(this.PathRetornoSituacaoErr);
            if (arqRetLote.Exists)
            {
                XElement root = XElement.Load(this.PathRetornoLote);
                foreach (var item in root.Elements("NumeroLoteGerado"))
                {
                    this.NumLoteGerado = item.Value.ToString();
                    this.GerouNumeroLoteOK = true;
                    retorno = true;
                }
            }
            else if (arqErr.Exists)
            {
                using (StreamReader sr = arqErr.OpenText())
                {
                    string s = "";
                    string resp = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        resp += s;
                       
                    }
                    UTF8Encoding utf88 = new UTF8Encoding();
                    byte[] byteArray = Encoding.ASCII.GetBytes(resp);
                    byte[] utf8Array = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, byteArray);
                    string finalString = utf88.GetString(utf8Array);
                    this.Resposta = finalString;
                   
                }
                this.GerouNumeroLoteOK = false;
                retorno = true;
            }
            else
            {
                this.GerouNumeroLoteOK = false;
                
                retorno = false;
                throw new Exception("Não foi possível obter o número do Lote. Verifique sua conexão com a internet e tente enviar novamente");
            }
            return retorno;
        }
        private void AtualizaNota()
        {
            RepositorioNotaFiscal rep = new RepositorioNotaFiscal();
            if (this.GerouNumeroLoteOK)
            {
                rep.AtualizarLote(this.Nota.Id, this.NumLoteGerado);
            }
            if (this.GerouNumeroReciboOK)
            {
                rep.AtualizarRecibo(this.Nota.Id, this.NumRecibo);
            }
            if (this.GerouProtocoloOK)
            {
                rep.AtualizarProtocolo(this.Nota.Id, this.NumProtocolo);
            }
            rep.AtualizarStatus(this.Nota.Id, this.Resposta);
            rep.AtualizarConfirmaEnvio(this.Nota.Id);
            
        }

        public void UsuarioDigitaChave(XmlDocument doc, string strUri, X509Certificate2 x509Certificado, string strArqXMLAssinado)
        {



            try
            {
                // Verifica o certificado a ser utilizado na assinatura
                string _xnome = "";
                if (x509Certificado != null)
                {
                    _xnome = x509Certificado.Subject.ToString();
                }

                X509Certificate2 _X509Cert = new X509Certificate2();
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, _xnome, false);

                if (collection1.Count == 0)
                {
                    throw new Exception("O UniNFe não conseguiu localizar nenhum certificado digital para assinatura e/ou envio do XML. Verifique as configurações.");
                }
                else
                {
                    //Verificar a validade do certificado
                    _X509Cert = null;
                    for (int i = 0; i < collection1.Count; i++)
                    {
                        //Verificar a validade do certificado
                        if (DateTime.Compare(DateTime.Now, collection1[i].NotAfter) == -1)
                        {
                            _X509Cert = collection1[i];
                            break;
                        }
                    }

                    //Se não encontrou nenhum certificado com validade correta, vou pegar o primeiro certificado, porem vai travar na hora de tentar enviar a nota fiscal, por conta da validade. Wandrey 06/04/2011
                    if (_X509Cert == null)
                        _X509Cert = collection1[0];

                    string x;
                    x = _X509Cert.GetKeyAlgorithm().ToString();

                    //Normalmente não consegue acessar no certificado A3, por que falta a digitação do PIN
                    //if (_X509Cert.PrivateKey == null)
                    //    throw new Exception("Não foi possível acessar a chave privada do certificado digital.");

                    // Create a new XML document.

                    // Format the document to ignore white spaces.
                    doc.PreserveWhitespace = false;

                    // Load the passed XML file using it’s name.
                    try
                    {
                        // Verifica se a tag a ser assinada existe é única
                        int qtdeRefUri = doc.GetElementsByTagName(strUri).Count;

                        if (qtdeRefUri == 0)
                        {
                            // a URI indicada não existe
                            throw new Exception("A tag de assinatura " + strUri.Trim() + " não existe no XML. (Código do Erro: 4)");
                        }
                        // Exsiste mais de uma tag a ser assinada
                        else
                        {
                            if (qtdeRefUri > 1)
                            {
                                // existe mais de uma URI indicada
                                throw new Exception("A tag de assinatura " + strUri.Trim() + " não é unica. (Código do Erro: 5)");
                            }
                            else
                            {
                                if (doc.GetElementsByTagName("Signature").Count == 0) //Documento ainda não assinado (Se já tiver assinado não vamos fazer nada, somente retornar ok para continuar o envio). Wandrey 12/05/2009
                                {
                                    try
                                    {
                                        // Create a SignedXml object.
                                        SignedXml signedXml = new SignedXml(doc);

                                        // Add the key to the SignedXml document
                                        signedXml.SigningKey = _X509Cert.PrivateKey;

                                        // Create a reference to be signed
                                        Reference reference = new Reference();

                                        // pega o uri que deve ser assinada
                                        XmlAttributeCollection _Uri = doc.GetElementsByTagName(strUri).Item(0).Attributes;
                                        foreach (XmlAttribute _atributo in _Uri)
                                        {
                                            if (_atributo.Name == "Id")
                                            {
                                                reference.Uri = "#" + _atributo.InnerText;
                                            }
                                        }

                                        // Add an enveloped transformation to the reference.
                                        XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                                        reference.AddTransform(env);

                                        XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                                        reference.AddTransform(c14);

                                        // Add the reference to the SignedXml object.
                                        signedXml.AddReference(reference);

                                        // Create a new KeyInfo object
                                        KeyInfo keyInfo = new KeyInfo();

                                        // Load the certificate into a KeyInfoX509Data object
                                        // and add it to the KeyInfo object.
                                        keyInfo.AddClause(new KeyInfoX509Data(_X509Cert));

                                        // Add the KeyInfo object to the SignedXml object.
                                        signedXml.KeyInfo = keyInfo;
                                        signedXml.ComputeSignature();

                                        // Get the XML representation of the signature and save
                                        // it to an XmlElement object.
                                        XmlElement xmlDigitalSignature = signedXml.GetXml();

                                        // Gravar o elemento no documento XML
                                        //doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                                        //XMLDoc = new XmlDocument();
                                        //XMLDoc.PreserveWhitespace = false;
                                        //XMLDoc = doc;

                                        // Atualizar a string do XML já assinada
                                        //this.vXMLStringAssinado = XMLDoc.OuterXml;

                                        // Gravar o XML Assinado no HD
                                        //StreamWriter SW_2 = File.CreateText(strArqXMLAssinado);
                                        //SW_2.Write(this.vXMLStringAssinado);
                                        //SW_2.Close();
                                    }
                                    catch (Exception caught)
                                    {
                                        throw (caught);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception caught)
                    {
                        throw (caught);
                    }
                }
            }
            catch (Exception caught)
            {
                throw (caught);
            }

        }
        
    }
}
