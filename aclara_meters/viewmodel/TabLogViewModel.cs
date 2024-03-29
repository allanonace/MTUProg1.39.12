﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MvvmHelpers;
using System.Threading.Tasks;
using aclara_meters.Models;
using MTUComm;
using aclara_meters;

namespace aclara.ViewModels
{
    public class TabLogViewModel: BaseViewModel
    {
        private const int HEIGH_TEXT_LIST = 22;
        public ObservableRangeCollection<ItemsLog> ItemsLog { get; } = new ObservableRangeCollection<ItemsLog>();
        private List<FileInfo> FileList = new List<FileInfo>();
        public string FileName { get; set; }
        public string FileDateTime { get; set;}
        public int IndexFile { get; set; }
        public int TotalFiles { get; set; }

        public TabLogViewModel()
        {
            RefreshList();
                     
        }

        public void RefreshList()
        {
            FileList = GenericUtilsClass.LogFilesToUpload(Mobile.LogUserPath,true,false,false);
            IndexFile = FileList.Count-1;
            TotalFiles = FileList.Count-1;
        }

        public async Task LoadData(int ind)
        {
            Stream stream=null;
            FileInfo file = FileList[ind];

            ItemsLog.Clear();
            var fileStream = new FileStream(file.FullName, FileMode.Open);
            stream = fileStream;
            ReadLogXML(stream);
            fileStream.Close();
            IndexFile = ind;
            FileName = file.Name;
            //FileDateTime = file.CreationTime.ToString("MM/dd/yyyy HH:00");
            FileDateTime = $"{FileName.Substring(0,2)}/{FileName.Substring(2, 2)}/{FileName.Substring(4, 4)} {FileName.Substring(8, 2)}:00";

        }

        private void ReadLogXML(Stream stream)
        {
            String sPort = String.Empty;
            String sAccion = String.Empty;
            String sError = String.Empty;
            String sIcon = String.Empty;
            String sDescripcion =String.Empty;
            String sValor = String.Empty;
            Boolean bTratar = false;
            String sSubAccion = String.Empty;
            String sSubIcon = String.Empty;
           
            XmlReader xReader = XmlReader.Create(stream);
            List<DatosAccion> ListaDatos = new List<DatosAccion>();
            List<DatosAccion> SubListaDatos = new List<DatosAccion>();
            List<ItemsLog> SubItemLogs = new List<ItemsLog>();
          
            while (xReader.Read())
            {
                if (xReader.Name == "Mtus") bTratar = true;
                if (xReader.Name == "Error") bTratar = true;
                if (bTratar)
                {
                    
                    switch (xReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xReader.Name == "Action")
                            {

                                if (xReader.Depth == 2)
                                {
                                    sAccion = xReader.GetAttribute("display");
                                    sIcon = "logs_file_button";
                                    ListaDatos = new List<DatosAccion>();
                                    SubItemLogs = new List<ItemsLog>();
                                }
                                else if (xReader.Depth == 3)
                                {
                                    sSubAccion = xReader.GetAttribute("display");
                                    sSubIcon = "logs_file_button";
                                    SubListaDatos = new List<DatosAccion>();
                                }
                            }
                            if (xReader.Name == "AppError")
                            {
                                sAccion = xReader.Name;
                                sIcon = "error";
                                ListaDatos = new List<DatosAccion>();
                            }

                            if (String.IsNullOrEmpty(xReader.GetAttribute("display")))
                                sDescripcion = xReader.Name;
                            else
                                sDescripcion = xReader.GetAttribute("display");

                            if (xReader.Name == "Message")
                            {
                                sError = xReader.GetAttribute("ErrorId");
                            }

                            if (xReader.Name == "Port")
                                sPort = xReader.GetAttribute("display");
                           
                            break;
                        case XmlNodeType.Text:
                            sValor = xReader.Value;
                            DatosAccion datosAccion = new DatosAccion();
                            if (!String.IsNullOrEmpty(sError) && sDescripcion == "Message")
                                datosAccion.Descripcion = String.Empty;
                            else if (String.IsNullOrEmpty(sPort))
                                datosAccion.Descripcion = $"{sDescripcion}:";
                            else
                                datosAccion.Descripcion = $" {sPort}-{sDescripcion}:";
                            
                            datosAccion.Valor = sValor;

                            if (!string.IsNullOrEmpty(sSubAccion))
                                SubListaDatos.Add(datosAccion);
                            else
                                ListaDatos.Add(datosAccion);
 
                            break;
                        case XmlNodeType.EndElement:

                            if (xReader.Name == "Action" )  // si acaba la accion
                            {
                                if (xReader.Depth == 2 && !String.IsNullOrEmpty(sAccion))
                                {
                                    ItemsLog Item = new ItemsLog
                                    {
                                        Accion = sAccion,
                                        Icon = sIcon
                                    };
                                    Item.HayLista = true;


                                    if (SubItemLogs.Count > 0)
                                    {
                                        ItemsLog Item1 = new ItemsLog
                                        {
                                            Accion = sAccion,
                                            Icon = sIcon
                                        };
                                        Item1.ListaDatos = ListaDatos;
                                        //Item1.HeightList = Item1.ListaDatos.Count > 5 ? (Item1.ListaDatos.Count * HEIGH_TEXT_LIST) : 100;
                                        Item.SubItemsLog = new List<ItemsLog>();
                                        Item.SubItemsLog.Add(Item1);
                                        Item.SubItemsLog.AddRange(SubItemLogs);
                                        Item.HayLista = false;                                    
                                    }
                                    else
                                    {
                                        Item.ListaDatos = ListaDatos;
                                        //Item.HeightList= Item.ListaDatos.Count>5?(Item.ListaDatos.Count * HEIGH_TEXT_LIST) : 100;
                                    }
                                    ItemsLog.Add(Item);
                                    sAccion = String.Empty;

                                }
                                else if(xReader.Depth==3)
                                {
                                    ItemsLog Item = new ItemsLog
                                    {
                                        Accion = sSubAccion,
                                        Icon = sSubIcon
                                    };

                                    Item.ListaDatos = SubListaDatos;
                                    //Item.HeightList = Item.ListaDatos.Count > 5 ? (Item.ListaDatos.Count * HEIGH_TEXT_LIST) : 100;
                                    SubItemLogs.Add(Item);
                                    sSubAccion = String.Empty;
                                }

                            }
                            if (xReader.Name == "AppError")  // si acaba la accion
                            {
                                ItemsLog Item = new ItemsLog
                                {
                                    Accion = $"Error {sError}",
                                    Icon = sIcon
                                };
                                Item.HayLista = true;
                                Item.ListaDatos = ListaDatos;
                                //Item.HeightList = 100;
                                ItemsLog.Add(Item);
                            }
                            if (xReader.Name == "Port")
                                sPort = String.Empty;
                            if (xReader.Name == "AppError")
                            {
                                sIcon = String.Empty;
                                sError = String.Empty;
                            }
                            if (xReader.Name == "Mtus" || xReader.Name == "Error")
                                bTratar = false;
                           
                            break;
                    }
                }
            }
            ItemsLog ItemNull = new ItemsLog
            {
                Accion = String.Empty,
                Icon = String.Empty
            };
            ItemsLog.Add(ItemNull);
            ItemNull = new ItemsLog
            {
                Accion = String.Empty,
                Icon = String.Empty
            };
            ItemsLog.Add(ItemNull);
          
        }

    }
}
