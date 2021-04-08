using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Avalonia.Controls;
using ReactiveUI;

namespace TestDataGridVirtualMasterDetail.Models
{
    public abstract class TreeViewItemModel: ReactiveUI.ReactiveObject, IDetailed, IColumned, IPictured, IAffected, INode
    {
        private string _DetailsChar = "⇊";

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

        public abstract int IconsColumnCount();
        public abstract ObservableCollection<Icon> Icons();
        public abstract INode GetParent();
        public abstract ObservableCollection<INode> GetChildren();
        public abstract Color GetColor();
        public abstract int ColumnsCount();
        public abstract ObservableCollection<string> ColumnValues();
    }

    public interface IColumned
    {
        int ColumnsCount();
        ObservableCollection<string> ColumnValues();
    }

    public interface IPictured
    {
        int IconsColumnCount();
        ObservableCollection<Icon> Icons();
    }

    public interface IAffected
    {
        
    }
    

    public interface INode
    {
        INode GetParent();
        ObservableCollection<INode> GetChildren();
        Color GetColor();
    }
    
    
    
    
    public interface IDetailed
    {
        bool DetailsVisible { get; set; }
        DataGridRow? BindedRow { get; set; }
        bool AlreadyFixed { get; set; }
        double? OldHeight { get; set; }
        public void HideDetails();
    }
}
