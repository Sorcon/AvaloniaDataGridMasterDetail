using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Reactive.Subjects;
using Avalonia.Controls;
using ReactiveUI;

namespace TestDataGridVirtualMasterDetail.Models
{
    public abstract class TreeViewItemModel: ReactiveUI.ReactiveObject, IDetailed, IColumned, IPictured, IAffected, INode
    {
        #region DetailsView

        private string _detailsChar = OpenChar;

        public bool DetailsVisible { get; set; } = false;
        public void ShowHideDetails()
        {
            DetailsVisible = !DetailsVisible;
            DetailsChar = DetailsVisible ? CloseChar : OpenChar;
            ShowHideViewDetails();
        }

        private static string CloseChar { get; set; } = "⇱";
        private static string OpenChar { get; set; } = "⇊";

        public string DetailsChar
        {
            get => _detailsChar;
            set => this.RaiseAndSetIfChanged(ref _detailsChar, value, nameof(DetailsChar));
        }
        public DataGridRow? BindedRow { get; set; } = null;
        public bool AlreadyFixed { get; set; }
        public double? OldHeight { get; set; }
        public void HideDetails()
        {
            DetailsVisible = false;
            DetailsChar = OpenChar;
        }

        private void ShowHideViewDetails()
        {
            if (BindedRow != null) BindedRow.AreDetailsVisible = DetailsVisible;
        }


        #endregion
        

        public abstract ObservableCollection<ColoredImage> Icons();
        public abstract INode Parent { get; set; }
        public abstract ObservableCollection<INode> Children { get; set; }
        public abstract Color Color { get; set; }

        private ColumnsMeta? _columnsMeta;
        public List<Subject<string>> ColumnValues { get; set; }



        public ref ColumnsMeta ColumnsMeta
        {
            get => ref _columnsMeta;
        }

        private void SetColumnsMeta(ref ColumnsMeta columnsMeta)
        {
            _columnsMeta = columnsMeta;
        }

        public ObservableCollection<EventModel> Affects { get; set; }
        public MemorySafer<string> AffectColorMemory { get; set; }
        public abstract string GetState();
    }

    public interface IColumned
    {
        List<Subject<string>> ColumnValues { get; set; }
        ref ColumnsMeta? ColumnsMeta { get; }
    }

    public interface IPictured
    {
        ObservableCollection<ColoredImage> Icons();
    }

    public interface IAffected
    {
        ObservableCollection<EventModel> Affects
        {
            get; set;
        }

        MemorySafer<string> AffectColorMemory { get; set; }

        string GetState();
    }
    

    public interface INode
    {
        INode Parent { get; set; }
        ObservableCollection<INode> Children { get; set; }
        Color Color { get; set; }
    }

    public class ColumnsMeta
    {
        public ColumnsMeta(List<string> columnNames)
        {
            ColumnNames = columnNames;
        }

        public int ColumnCount => ColumnNames.Count;
        private List<string> ColumnNames { get;}
        public List<string> GetColumnNames() => ColumnNames;
    }

    public class ColoredImage
    {
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Tooltip { get; set; }
    }
    
    
    public interface IDetailed
    {
        bool DetailsVisible { get; set; }
        DataGridRow? BindedRow { get; set; }
        bool AlreadyFixed { get; set; }
        double? OldHeight { get; set; }
        public void HideDetails();
    }

    public class TreeViewConfiguration
    {
        public string CloseChar { get; set; } = "⇱";
        public string OpenChar { get; set; } = "⇊";
        public uint TreeLevel { get; set; } = 3;
        public ColumnsMeta ColumnsMeta { get; set; } = new ColumnsMeta(new List<string>(){"Имя", "Тип", "Состояние"});
    }
    
    public class MemorySafer<T> : INotifyPropertyChanged
    {
        public delegate void ValueUpdateHandler(T newVal);


        public event PropertyChangedEventHandler PropertyChanged;
        public event ValueUpdateHandler ValueChanged;
        public MemorySafer(T value)
        {
            Value = value;
        }
        public T Value { get; private set; }

        public void SetValue(T val)
        {
            this.Value = val;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            this.ValueChanged?.Invoke(val);
        }

    }

    public class EventModel
    {
        
    }
}
