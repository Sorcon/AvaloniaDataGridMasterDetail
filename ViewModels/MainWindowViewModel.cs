using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Avalonia.Controls;
using ReactiveUI;
using TestDataGridVirtualMasterDetail.Views;

namespace TestDataGridVirtualMasterDetail.ViewModels
{



    public class SecondL : ReactiveUI.ReactiveObject, IDetailed
    {
        private string _DetailsChar = "⇊";
        public string TestColumn2 { get; init; } = "";
        public string RowDetail2 { get; init; } = "";
        public ObservableCollection<ThirdL> TestDetails2 { get; set; } = new ObservableCollection<ThirdL>();
        public bool DetailsVisible { get; set; } = false;
        public void ShowHideDetails()
        {
            DetailsVisible = !DetailsVisible;
            DetailsChar = DetailsVisible ? "⇱" : "⇊";
            ShowHideViewDetails();
        }
        public string DetailsChar
        {
            get => _DetailsChar;
            set => this.RaiseAndSetIfChanged(ref _DetailsChar, value, nameof(DetailsChar));
        }
        public DataGridRow? BindedRow { get; set; } = null;
        public bool AlreadyFixed { get; set; }
        public double? OldHeight { get; set; }
        public void HideDetails()
        {
            DetailsVisible = false;
            DetailsChar = "⇊";
        }

        private void ShowHideViewDetails()
        {
            if (BindedRow != null) BindedRow.AreDetailsVisible = DetailsVisible;
        }
    }

    public class ThirdL : ReactiveUI.ReactiveObject, IDetailed
    {
        public string TestColumn3 { get; init; } = "";
        public string Counter { get; set; } = "";
        public bool DetailsVisible { get; set; } = false;
        public DataGridRow? BindedRow { get; set; } = null;
        public bool AlreadyFixed { get; set; }
        public double? OldHeight { get; set; }
        public void HideDetails()
        {
            
        }
    }

    public class FirstL : ReactiveUI.ReactiveObject, IDetailed
    {


        private string _DetailsChar = "⇊";
        public string TestColumn { get; init; } = "";
        public string RowDetail1 { get; init; } = "";
        public ObservableCollection<SecondL> TestDetails1 { get; set; } = new ObservableCollection<SecondL>();
        public bool DetailsVisible { get; set; } = false;

        public string DetailsChar
        {
            get => _DetailsChar;
            set => this.RaiseAndSetIfChanged(ref _DetailsChar, value, nameof(DetailsChar));
        }

        public void HideDetails()
        {
            DetailsVisible = false;
            DetailsChar = "⇊";
        }

        public void ShowHideDetails()
        {
            DetailsVisible = !DetailsVisible;
            DetailsChar = DetailsVisible ? "⇱" : "⇊";
            ShowHideViewDetails();
        }
        public DataGridRow? BindedRow { get; set; } = null;
        public bool AlreadyFixed { get; set; }
        public double? OldHeight { get; set; }

        private void ShowHideViewDetails()
        {
            if (BindedRow != null) BindedRow.AreDetailsVisible = DetailsVisible;
        }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        
        private ObservableCollection<FirstL> _NotFilteredItems;
        
        public ObservableCollection<FirstL> TestItems
        {
            get => String.IsNullOrEmpty(_FilterText) ? _NotFilteredItems : new(_NotFilteredItems.Where(t=>t.RowDetail1.Contains(_FilterText)).ToList()) ;
            set => _NotFilteredItems = value;
        }

        private SecondL GenerateSection(string caption, int sectionId)
        {
            var result = new SecondL()
            {
                TestColumn2 = $"{caption} section {sectionId}",
                RowDetail2 = $"{caption} section {sectionId}"
            };
            result.TestDetails2 = new ObservableCollection<ThirdL>();
            result.TestDetails2.Add(GenerateZone(caption,sectionId,1));
            result.TestDetails2.Add(GenerateZone(caption,sectionId,2));
            result.TestDetails2.Add(GenerateZone(caption,sectionId,3));
            return result;
        }

        private ThirdL GenerateZone(string caption, int sectionId, int zoneId)
        {
            var result = new ThirdL()
            {
                TestColumn3 = $"{caption} section {sectionId} zone {zoneId}",
            };
            return result;
        }

        private void GenerateMockItems(string caption)
        {
            var tItem = new FirstL() { TestColumn = $"{caption}", RowDetail1 = $"{caption}" };
            tItem.TestDetails1 = new ObservableCollection<SecondL>();
            tItem.TestDetails1.Add(GenerateSection(caption,1));
            tItem.TestDetails1.Add(GenerateSection(caption,2));
            tItem.TestDetails1.Add(GenerateSection(caption,3));
            TestItems.Add(tItem);
        }
        private string _FilterText;

        public string FilterText { 
            get => _FilterText;
            set
            {
                this.RaiseAndSetIfChanged(ref _FilterText, value, nameof(FilterText));
                Filter();
            }
        }

        private void Filter()
        {
            this.RaisePropertyChanged(nameof(TestItems));
        }

        private void GenerateCounterTick ()
        {
            var i2 = 0;

            for (; ; )
            {
                i2++;
                foreach (var first in TestItems)
                    foreach (var second in first.TestDetails1)
                        foreach (var third in second.TestDetails2)
                        {

                            third.Counter = i2.ToString();
                            third.RaisePropertyChanged($"Counter");
                        }
               
            }
        }

        public MainWindowViewModel()
        {
            TestItems = new ObservableCollection<FirstL>();

            for(int i = 1; i<=10000; i++)
            {
                GenerateMockItems($"device_{i}");
                GenerateMockItems($"other_device_{i}");
                GenerateMockItems($"big_device_{i}");
            }

            Task.Run(GenerateCounterTick);
        }
    }
}
