using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using DynamicData;
using TestDataGridVirtualMasterDetail.Models;

namespace TestDataGridVirtualMasterDetail.Views
{
    public class TreeViewControl : UserControl
    {
        private ObservableCollection<TreeViewItemModel> _tree;
        public ObservableCollection<TreeViewItemModel> TreeItems => _tree;
        private ColumnsMeta _columnsMeta;
        public TreeViewControl(ObservableCollection<TreeViewItemModel> tree, ColumnsMeta columnsMeta)
        {
            _tree = tree;
            _columnsMeta = columnsMeta;
            InitializeComponent();
            BuildVisualTree();
        }

        private Grid _controlGrid { get; set; }

        private void BuildVisualTree()
        {
            _controlGrid = this.FindControl<Grid>("ControlGrid");
            var firstDataGrid = InitDataGrid("TreeItems");
            firstDataGrid.DataContext = this;
        }

        private DataGrid InitDataGrid(string itemsBinding = "Children")
        {
            var result = new DataGrid
            {
                [!DataGrid.ItemsProperty] = new Binding(itemsBinding),
                RowDetailsTemplate = GenerateDetailsTemplate(),

            };
            result.Columns.AddRange(GenerateColumns());
            Grid.SetColumnSpan(result,2);
            result.CanUserResizeColumns = true;
            result.LoadingRow += FirstDataGridOnLoadingRow;
            result.UnloadingRow += FirstDataGridOnUnloadingRow;
            result.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
            result.SelectionMode = DataGridSelectionMode.Single;
            result.GridLinesVisibility = DataGridGridLinesVisibility.All;
            result.IsReadOnly = true;
            
            return result;
        }

        private IDataTemplate GenerateDetailsTemplate()
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<DataGridColumn> GenerateColumns()
        {
            var result = new List<DataGridColumn>();
            result.Add(GenerateButtonColumn());
            result.AddRange(GenerateTextColumns());
            return result;
        }

        private IEnumerable<DataGridColumn> GenerateTextColumns()
        {
            var result = new List<DataGridColumn>();
            foreach (var columnName in _columnsMeta.GetColumnNames())
            {
                
            }

            ;
            return result;
        }

        private DataGridColumn GenerateButtonColumn()
        {
            var result = new DataGridTemplateColumn();
            var cTemplate = new DataTemplate();
            var button = new Button()
            {

                Name = "SHButton",
                [!ContentProperty] = new Binding("DetailsChar"),
                [!Button.CommandProperty] = new Binding("ShowHideDetails")
            };
           
            cTemplate.Content = button;
            result.CellTemplate = cTemplate;
            
            return result;
        }

        private void FirstDataGridOnUnloadingRow(object? sender, DataGridRowEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void FirstDataGridOnLoadingRow(object? sender, DataGridRowEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private TreeViewControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}