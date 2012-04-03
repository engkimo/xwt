﻿//
// ListBoxBackend.cs
//
// Author:
//       Eric Maupin <ermau@xamarin.com>
//
// Copyright (c) 2012 Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xwt.Backends;
using Xwt.WPFBackend.Utilities;

namespace Xwt.WPFBackend
{
	public class ListBoxBackend
		: WidgetBackend, IListBoxBackend
	{
		public ListBoxBackend()
		{
			ListBox = new ExListBox();
		}

		public ScrollPolicy VerticalScrollPolicy
		{
			get { return ScrollViewer.GetVerticalScrollBarVisibility (ListBox).ToXwtScrollPolicy(); }
			set { ScrollViewer.SetVerticalScrollBarVisibility (ListBox, value.ToWpfScrollBarVisibility ()); }
		}

		public ScrollPolicy HorizontalScrollPolicy
		{
			get { return ScrollViewer.GetHorizontalScrollBarVisibility (ListBox).ToXwtScrollPolicy (); }
			set { ScrollViewer.SetVerticalScrollBarVisibility (ListBox, value.ToWpfScrollBarVisibility ()); }
		}

		public void SetViews (CellViewCollection views)
		{
			ListBox.ItemTemplate = new DataTemplate { VisualTree = CellUtil.CreateBoundColumnTemplate (views) };
		}

		public void SetSource (IListDataSource source, IBackend sourceBackend)
		{
			var dataSource = sourceBackend as ListDataSource;
			if (dataSource != null)
				ListBox.ItemsSource = dataSource;
			else
				ListBox.ItemsSource = new ListSourceNotifyWrapper (source);
		}

		public void SetSelectionMode (SelectionMode mode)
		{
			switch (mode) {
			case SelectionMode.Single:
				ListBox.SelectionMode = System.Windows.Controls.SelectionMode.Single;
				break;
			case SelectionMode.Multiple:
				ListBox.SelectionMode = System.Windows.Controls.SelectionMode.Extended;
				break;
			}
		}

		public void SelectAll ()
		{
			ListBox.SelectAll();
		}

		public void UnselectAll ()
		{
			ListBox.UnselectAll();
		}

		public int[] SelectedRows
		{
			get { return ListBox.SelectedIndexes.ToArray (); }
		}

		public void SelectRow (int pos)
		{
			ListBox.SelectedIndexes.Add (pos);
		}

		public void UnselectRow (int pos)
		{
			ListBox.SelectedIndexes.Remove (pos);
		}

		protected ExListBox ListBox {
			get { return (ExListBox) Widget; }
			set { Widget = value; }
		}
	}
}