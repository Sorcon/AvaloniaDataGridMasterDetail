using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

using TestDataGridVirtualMasterDetail.ViewModels;

namespace TestDataGridVirtualMasterDetail.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var g = this.FindControl<DataGrid>("MainDataGrid");
            g.ApplyTemplate();

        }

        private void OnLoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var row = e.Row;
            if (row.DataContext is not IDetailed detailed) return;
            /*row.AreDetailsVisible = detailed.DetailsVisible;
            var p = row.Parent;
            DataGrid? dg = null;
            while (p is not DataGrid && p is not null)
            {
                p = p.Parent;
            }
            if(p is DataGrid grid)
                dg = grid;
            if (detailed.OldHeight is not null && dg is not null)
            {
                dg.Height = detailed.OldHeight.Value;
            }*/
            detailed.BindedRow = row;
        }

        private void OnUnloadingRow(object? sender, DataGridRowEventArgs e)
        {
            var row = e.Row;
            row.AreDetailsVisible = false;
            if (row.DataContext is IDetailed detailed)
            {
                detailed.HideDetails();
                
                detailed.AlreadyFixed = false;
                detailed.BindedRow = null;
            }
        }

        private void DataGrid_OnRowDetailsVisibilityChanged(object? sender, DataGridRowDetailsEventArgs e)
        {
            var opened = e.Row.AreDetailsVisible;
            var data = e.Row.DataContext as IDetailed;
            double? result = null;

            var afixed = data is {AlreadyFixed: true};
            data.AlreadyFixed = true;
            var dg = (sender as DataGrid);
            var detailsStackPanel = e.DetailsElement as StackPanel;
            var detHeight =  RecalculateHeights(detailsStackPanel, afixed);
            var detHeight2 = RecalculateHeights2(detailsStackPanel);
            if (opened)
            {
               
                if(dg is {TransformedBounds: { }})
                {
                    result =  dg.TransformedBounds.Value.Bounds.Height + detHeight;
                }
                else
                {
                    result = data.OldHeight;
                }
            }
            else
            {
                if(dg is {TransformedBounds: { }})
                {
                    result =  dg.TransformedBounds.Value.Bounds.Height - detHeight2;
                }
                else
                {
                    if (dg != null) result = dg.Height -  detHeight2;
                }
            }

            if (!result.HasValue) return;
            dg.Height = result.Value;
            data.OldHeight = result.Value;
        }

        private double RecalculateHeights2(StackPanel? sp)
        {
            double result = 0;
            foreach (var visual in sp.GetVisualChildren())
            {
                var el = (ILayoutable) visual;
                if (el is not DataGrid dg)
                {
                    result += el.Height;
                    continue;
                }
                var prop =
                    typeof(DataGrid).GetProperty("EdgedRowsHeightCalculated", BindingFlags.NonPublic | BindingFlags.Instance);

                var getter = prop?.GetGetMethod(nonPublic: true);

                if(getter?.Invoke(dg, null) is double val)
                    result += val;

            }

            return result;
        }

        private double RecalculateHeights(StackPanel? sp, bool isFixed)
        {
            double result = 0;
            foreach (var visual in sp.GetVisualChildren())
            {
                var el = (ILayoutable) visual;

                if (el is not DataGrid dg)
                {
                    result += el.Height is 0 or double.NaN ? el.DesiredSize.Height : el.Height;
                    continue;
                }

                
                result += el.DesiredSize.Height;

                if (isFixed) continue;
                var prop =
                    typeof(DataGrid).GetProperty("EdgedRowsHeightCalculated", BindingFlags.NonPublic | BindingFlags.Instance);

                var getter = prop?.GetGetMethod(nonPublic: true);

                if(getter?.Invoke(dg, null) is double val)
                    result += val;


            }

            return result;
        }

        private void OnVerticalScroll(object? sender, ScrollEventArgs e)
        {
            var dg = sender as DataGrid;
            
        }
    }
    
    
    


    internal interface IDetailed
    {
        bool DetailsVisible { get; set; }
        DataGridRow? BindedRow { get; set; }
        bool AlreadyFixed { get; set; }
        double? OldHeight { get; set; }
        public void HideDetails();
    }
}