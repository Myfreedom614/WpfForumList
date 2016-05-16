using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace WpfForumList
{
    /// <summary>
    /// Forum Enum Type
    /// </summary>
    enum Forums { MSDN, MSDNCHS, TechNet, TechNetCHS };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMSDNNeedDownload = false;
        private bool isTechNetNeedDownload = false;

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitialSomething();

            InitializeComponent();

            this.Title += " V"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            CheckDownload(isMSDNNeedDownload, isTechNetNeedDownload);
        }

        #region Update TreeView DataContext and Binding
        /// <summary>
        /// Update TreeView MSDN DataContext and Binding
        /// </summary>
        private void UpdateTVMSDNDataContextBinding()
        {
            XmlDataProvider xdp = new XmlDataProvider() { Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml"), XPath = "select/optgroup" };
            try
            {
                tv_MSDN.DataContext = xdp;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Binding NewBinding = new Binding();
            NewBinding.Source = xdp;
            tv_MSDN.SetBinding(TreeView.ItemsSourceProperty, NewBinding);
        }

        /// <summary>
        /// Update TreeView MSDNCHS DataContext and Binding
        /// </summary>
        private void UpdateTVMSDNCHSDataContextBinding()
        {
            XmlDataProvider xdp = new XmlDataProvider() { Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml"), XPath = "select/optgroup" };
            try
            {
                tv_MSDN.DataContext = xdp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Binding NewBinding = new Binding();
            NewBinding.Source = xdp;
            tv_MSDN.SetBinding(TreeView.ItemsSourceProperty, NewBinding);
        }

        /// <summary>
        /// Update TreeView TechNet DataContext and Binding
        /// </summary>
        private void UpdateTVTechNetDataContextBinding()
        {
            XmlDataProvider xdp = new XmlDataProvider() { Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml"), XPath = "select/optgroup" };
            try { 
                tv_TechNet.DataContext = xdp;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Binding NewBinding = new Binding();
            NewBinding.Source = xdp;
            tv_TechNet.SetBinding(TreeView.ItemsSourceProperty, NewBinding);
        }

        /// <summary>
        /// Update TreeView TechNet CHS DataContext and Binding
        /// </summary>
        private void UpdateTVTechNetCHSDataContextBinding()
        {
            XmlDataProvider xdp = new XmlDataProvider() { Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml"), XPath = "select/optgroup" };
            try
            {
                tv_TechNet.DataContext = xdp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Binding NewBinding = new Binding();
            NewBinding.Source = xdp;
            tv_TechNet.SetBinding(TreeView.ItemsSourceProperty, NewBinding);
        }

        #endregion

        /// <summary>
        /// Do initialization
        /// </summary>
        private void InitialSomething()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml") == false)
            {
                //MessageBox.Show("MSDNForum.xml is missing");
                isMSDNNeedDownload = true;
            }
            else
                EmptyXMLNode(Forums.MSDN);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml") == false)
            {
                //MessageBox.Show("MSDNCHSForum.xml is missing");
                isMSDNNeedDownload = true;
            }
            else
                EmptyXMLNode(Forums.MSDNCHS);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml") == false)
            {
                //MessageBox.Show("TechNetForum.xml is missing");
                isTechNetNeedDownload = true;
            }
            else
                EmptyXMLNode(Forums.TechNet);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml") == false)
            {
                //MessageBox.Show("TechNetCHSForum.xml is missing");
                isTechNetNeedDownload = true;
            }
            else
                EmptyXMLNode(Forums.TechNetCHS);
        }

        /// <summary>
        /// Check If Need to Open Download Window
        /// </summary>
        /// <param name="isMSDNNeedDownload">Flag: MSDN</param>
        /// <param name="isTechNetNeedDownload">Flag: TechNet</param>
        private void CheckDownload(bool isMSDNNeedDownload, bool isTechNetNeedDownload)
        {
            if (isMSDNNeedDownload || isTechNetNeedDownload)
            {
                DataDownload w = new DataDownload(isMSDNNeedDownload, isTechNetNeedDownload);
                if (w.ShowDialog() == false)
                {
                    if (isMSDNNeedDownload && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml"))
                    {
                        //Update DataContext and Binding
                        UpdateTVMSDNDataContextBinding();
                    }
                    else if (isMSDNNeedDownload && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml"))
                    {
                        //Update DataContext and Binding
                        UpdateTVMSDNCHSDataContextBinding();
                    }

                    if (isTechNetNeedDownload && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml"))
                    {
                        //Update DataContext and Binding
                        UpdateTVTechNetDataContextBinding();
                    }
                    else if (isTechNetNeedDownload && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml"))
                    {
                        //Update DataContext and Binding
                        UpdateTVTechNetCHSDataContextBinding();
                    }
                }
            }
        }

        #region Processing XML File
        /// <summary>
        /// Empty XML Node Count
        /// </summary>
        /// <param name="forum"></param>
        private void EmptyXMLNode(Enum forum)
        {
            string filename = forum.ToString();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filename + "Forum.xml");
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("optgroup");
                foreach (XmlNode category in nodes)
                {
                    //Reset Attribute
                    if (category.Attributes["count"]!=null)
                        category.Attributes["count"].Value = "0";
                    else
                    {
                        //Add Attribute
                        XmlAttribute typeAttr = doc.CreateAttribute("count");
                        typeAttr.Value = "0";
                        category.Attributes.Append(typeAttr);
                    }
                }

                doc.Save(filename + "Forum.xml");
            } catch(Exception ex)
            {
                MessageBox.Show("Oops, Load " + filename + " Forum Data File Failed: " + ex.Message + "\r\n\r\nPlease try to download data file again:(");
                //Need add CHS support
                if (filename == Forums.MSDN.ToString())
                { 
                    isMSDNNeedDownload = true;
                    if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml");
                    }
                }
                if (filename == Forums.MSDNCHS.ToString())
                {
                    isMSDNNeedDownload = true;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml");
                    }
                }
                if (filename == Forums.TechNet.ToString())
                { 
                    isTechNetNeedDownload = true;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml");
                    }
                }
                if (filename == Forums.TechNetCHS.ToString())
                {
                    isTechNetNeedDownload = true;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml");
                    }
                }
            }
            
        }

        /// <summary>
        /// Calculate The Number of Results Inside a Node
        /// </summary>
        /// <param name="forum"></param>
        private void CalcXMLNode(Enum forum)
        {
            string filename = forum.ToString();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filename + "Forum.xml");
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("optgroup");
                foreach (XmlNode category in nodes)
                {
                    int numberOfItems = 0;
                    string keyword=TxtSearchText.Text;
                    if (keyword.Contains("+"))
                        keyword = keyword.Replace("+", "\\+");
                    Regex regex = new Regex("(" + keyword + ")", RegexOptions.IgnoreCase);

                    foreach (XmlNode xn in category.SelectNodes("option"))
                    {
                        string[] substrings = regex.Split(xn.Attributes["label"].Value);
                        foreach (var item in substrings)
                        {
                            if (regex.Match(item).Success)
                            {
                                numberOfItems++;
                                break;
                            }
                        }
                    }

                    if(category.Attributes["count"]!=null)
                    {
                        category.Attributes["count"].Value = numberOfItems.ToString();
                    }
                    else
                    { 
                        //Add Attribute
                        XmlAttribute typeAttr = doc.CreateAttribute("count");
                        typeAttr.Value = numberOfItems.ToString();
                        category.Attributes.Append(typeAttr);
                    }
                    Debug.WriteLine(category.Attributes["label"].Value + " : " + numberOfItems);
                }

                doc.Save(filename + "Forum.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oops, Something is wrong: " + ex.Message);
                //Need add CHS support
                if (filename == Forums.MSDN.ToString())
                {
                    isMSDNNeedDownload = true;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml");
                    }
                }
                if (filename == Forums.MSDNCHS.ToString())
                {
                    isMSDNNeedDownload = true;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml");
                    }
                }
                if (filename == Forums.TechNet.ToString())
                {
                    isTechNetNeedDownload = true;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml");
                    }
                }
                if (filename == Forums.TechNetCHS.ToString())
                {
                    isTechNetNeedDownload = true;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml"))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml");
                    }
                }
                CheckDownload(isMSDNNeedDownload, isTechNetNeedDownload);
            }
        }

        #endregion

        #region Highlight Staff

        /// <summary>
        /// Find Item in Control
        /// </summary>
        /// <param name="obj"></param>
        public void FindControlItem(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                TreeViewItem tv = obj as TreeViewItem;

                if (tv != null)
                {
                    HighlightText(tv);
                }
                FindControlItem(VisualTreeHelper.GetChild(obj as DependencyObject, i));
            }
        }

        /// <summary>
        /// Highlight Text
        /// </summary>
        /// <param name="itx"></param>
        private void HighlightText(Object itx)
        {
            if (itx != null)
            {
                if (itx is TextBlock)
                {
                    string keyword = TxtSearchText.Text;
                    if (keyword.Contains("+"))
                        keyword = keyword.Replace("+", "\\+");
                    Regex regex = new Regex("(" + keyword + ")", RegexOptions.IgnoreCase);
                    TextBlock tb = itx as TextBlock;
                    if (TxtSearchText.Text.Length == 0)
                    {
                        string str = tb.Text;
                        tb.Inlines.Clear();
                        tb.Inlines.Add(str);
                        return;
                    }
                    string[] substrings = regex.Split(tb.Text);
                    tb.Inlines.Clear();
                    foreach (var item in substrings)
                    {
                        if (regex.Match(item).Success)
                        {
                            Run runx = new Run(item);
                            runx.Background = Brushes.Yellow;
                            tb.Inlines.Add(runx);
                        }
                        else
                        {
                            tb.Inlines.Add(item);
                        }
                    }
                    return;
                }
                else
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(itx as DependencyObject); i++)
                    {
                        HighlightText(VisualTreeHelper.GetChild(itx as DependencyObject, i));
                    }
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Read XML files And Expand items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            string filepath = string.Empty;
            if (cbxMSDN.SelectedIndex == 0)
                filepath = AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml";
            else
                filepath = AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml";

            if (ChkMSDN.IsChecked == true && File.Exists(filepath))
            {
                int totalMSDN = 0;
                foreach (TreeViewItem tvi in FindVisualChildren<TreeViewItem>(tv_MSDN))
                {
                    try
                    {
                        if (((System.Xml.XmlElement)(((System.Windows.Controls.HeaderedItemsControl)(tvi)).Header)).Attributes["count"].Value != "0")
                        {
                            totalMSDN += Convert.ToInt32(((System.Xml.XmlElement)(((System.Windows.Controls.HeaderedItemsControl)(tvi)).Header)).Attributes["count"].Value);
                            tvi.IsExpanded = true;
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }

                //Show total results
                labelMSDN.Content = String.Format("{0} results found",totalMSDN);
                FindControlItem(tv_MSDN);
            }

            if (cbxTechNet.SelectedIndex == 0)
                filepath = AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml";
            else
                filepath = AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml";

            if (ChkTechNet.IsChecked == true && File.Exists(filepath))
            {
                int totalTechNet= 0;
                foreach (TreeViewItem tvi in FindVisualChildren<TreeViewItem>(tv_TechNet))
                {
                    try
                    {
                        if (((System.Xml.XmlElement)(((System.Windows.Controls.HeaderedItemsControl)(tvi)).Header)).Attributes["count"].Value != "0")
                        {
                            totalTechNet += Convert.ToInt32(((System.Xml.XmlElement)(((System.Windows.Controls.HeaderedItemsControl)(tvi)).Header)).Attributes["count"].Value);
                            tvi.IsExpanded = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }

                //Show total results
                labelTechNet.Content = String.Format("{0} results found", totalTechNet);
                FindControlItem(tv_TechNet);
            }

            BtnSearchText.IsEnabled = true;
            cbxMSDN.IsEnabled = true;
            cbxTechNet.IsEnabled = true;
            this.Cursor = Cursors.Arrow;
        }
        #endregion

        #region Control's Event handler

        /// <summary>
        /// Send Feedback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:v-frche@microsoft.com?subject=[Feedback] Microsoft Forum List" + " V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "&body=Hi Franklin,";
            proc.Start();
        }

        /// <summary>
        /// Double Click MSDN TreeView Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEvtArgs"></param>
        private void OnMSDNTreeNodeDoubleClick(object sender, MouseButtonEventArgs mouseEvtArgs)
        {
            XmlAttribute xa = ((XmlElement)(((HeaderedItemsControl)(sender)).Header)).Attributes["value"];
            if (xa != null)
            {
                string flink = string.Empty;
                if (cbxMSDN.SelectedIndex == 0)
                    flink = "http://social.msdn.microsoft.com/Forums/en-US/home?forum=" + xa.Value;
                else
                    flink = "http://social.msdn.microsoft.com/Forums/zh-CN/home?forum=" + xa.Value;
                
                //MessageBox.Show(flink);
                Process.Start(flink);
            }
        }

        /// <summary>
        /// Copy Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Set text to clip board 
            TreeView tvi = (TreeView)sender;

            XmlAttribute xa = ((XmlElement)(tvi.SelectedItem)).Attributes["value"];
            if (xa != null)
            {
                string url_Template = "http://social.{0}.microsoft.com/Forums/en-US/home?forum={1}";
                string flink = string.Empty;
                if (tvi.Name == "tv_MSDN")
                    flink = string.Format(url_Template, "msdn", xa.Value);
                else
                    flink = string.Format(url_Template, "technet", xa.Value);
                //MessageBox.Show(flink);
                Clipboard.SetText(flink);
            }

        }

        /// <summary>
        /// Double Click TechNet TreeView Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEvtArgs"></param>
        private void OnTechNetTreeNodeDoubleClick(object sender, MouseButtonEventArgs mouseEvtArgs)
        {
            XmlAttribute xa = ((XmlElement)(((HeaderedItemsControl)(sender)).Header)).Attributes["value"];
            if (xa != null)
            {
                string flink = string.Empty;
                if (cbxTechNet.SelectedIndex == 0)
                    flink = "http://social.technet.microsoft.com/Forums/en-US/home?forum=" + xa.Value;
                else
                    flink = "http://social.technet.microsoft.com/Forums/zh-CN/home?forum=" + xa.Value;

                //MessageBox.Show(flink);
                Process.Start(flink);
            }
        }

        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearchText_Click(object sender, RoutedEventArgs e)
        {
            BtnSearchText.IsEnabled = false;
            cbxMSDN.IsEnabled = false;
            cbxTechNet.IsEnabled = false;
            this.Cursor = Cursors.Wait;

            string filepath = string.Empty;

            if (cbxMSDN.SelectedIndex == 0)
                filepath = AppDomain.CurrentDomain.BaseDirectory + "MSDNForum.xml";
            else
                filepath = AppDomain.CurrentDomain.BaseDirectory + "MSDNCHSForum.xml";

            if (ChkMSDN.IsChecked == true && File.Exists(filepath))
            {
                if (cbxMSDN.SelectedIndex == 0)
                { 
                    CalcXMLNode(Forums.MSDN);

                    //Update DataContext and Binding
                    UpdateTVMSDNDataContextBinding();
                }
                else
                {
                    CalcXMLNode(Forums.MSDNCHS);

                    //Update DataContext and Binding
                    UpdateTVMSDNCHSDataContextBinding();
                }
            }

            if (cbxTechNet.SelectedIndex == 0)
                filepath = AppDomain.CurrentDomain.BaseDirectory + "TechNetForum.xml";
            else
                filepath = AppDomain.CurrentDomain.BaseDirectory + "TechNetCHSForum.xml";

            if (ChkTechNet.IsChecked == true && File.Exists(filepath))
            {
                if (cbxTechNet.SelectedIndex == 0)
                {
                    CalcXMLNode(Forums.TechNet);

                    //Update DataContext and Binding
                    UpdateTVTechNetDataContextBinding();
                }
                else
                {
                    CalcXMLNode(Forums.TechNetCHS);

                    //Update DataContext and Binding
                    UpdateTVTechNetCHSDataContextBinding();
                }
                
            }

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Handle Enter Key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearchText_Click(null, null);
            }
        }

        /// <summary>
        /// MSDN ComboBox SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMSDN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbxMSDN.SelectedIndex==0)
            {
                UpdateTVMSDNDataContextBinding();
            }
            else
            {
                UpdateTVMSDNCHSDataContextBinding();
            }
        }

        /// <summary>
        /// TechNet ComboBox SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTechNet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTechNet.SelectedIndex == 0)
            {
                UpdateTVTechNetDataContextBinding();
            }
            else
            {
                UpdateTVTechNetCHSDataContextBinding();
            }
        }

        #endregion

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            cbxMSDN.SelectionChanged+=cbxMSDN_SelectionChanged;
            cbxTechNet.SelectionChanged += cbxTechNet_SelectionChanged;
        }

    }
}
