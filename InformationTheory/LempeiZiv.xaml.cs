using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

using System.Collections;
using System.Text;

namespace SharpLZW
{
	public static class LZWEncoder
	{
		public static string FillWithZero(this string value, int len)
		{
			while (value.Length < len)
			{
				value = "0" + value;
			}

			return value;
		}

		public static string Encode(string input)
		{
			int codeLen = 0;
			Dictionary<string, int> dict = new Dictionary<string, int>();
			dict.Add("", 0);
			StringBuilder sb = new StringBuilder();

			int i = 0;
			string w = "";
			while (i < input.Length)
			{
				w = input[i].ToString();

				i++;

				while (dict.ContainsKey(w) && i < input.Length)
				{
					w += input[i];
					i++;
				}

				if (dict.ContainsKey(w) == false)
				{
					if (i == 1)
					{
						sb.Append(w);
						sb.Append(",");
					}
					else
					{
						string matchKey = w.Substring(0, w.Length - 1);
						sb.Append(Convert.ToString(dict[matchKey], 2).FillWithZero(codeLen));
						sb.Append(w.Last());
						sb.Append(",");
					}


					if (Convert.ToString(dict.Count, 2).Length > codeLen)
						codeLen++;

					dict.Add(w, dict.Count);
				}
				else
				{
					sb.Append(Convert.ToString(dict[w], 2).FillWithZero(codeLen));

					if (Convert.ToString(dict.Count, 2).Length > codeLen)
						codeLen++;

				}
			}

			return sb.ToString();
		}
	}
}

namespace InformationTheory
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LempeiZiv : Page
	{
		public LempeiZiv()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void inputGotFocus(object sender, RoutedEventArgs e)
		{
			input.SelectAll();
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			output.Text = SharpLZW.LZWEncoder.Encode(input.Text);
		}
	}
}
