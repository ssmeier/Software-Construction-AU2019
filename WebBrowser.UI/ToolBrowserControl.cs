﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebBrowser.Logic.New;

namespace WebBrowser.UI
{
    public partial class ToolBrowserControl : UserControl
    {
        
        public ToolBrowserControl()
        {
            InitializeComponent();
        }

        SessionHistory session = new SessionHistory();

        private void goButton_Click(object sender, EventArgs e)
        {
            
                try
                {
                    Uri targetUrl = new Uri(urlBox.Text);
                    webBrowser1.Navigate(targetUrl);
                    
                }
                catch { }
            
        }

        private void urlBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                goButton_Click(new object(), new EventArgs());
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(session.PageForward(webBrowser1.Url));
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(session.PageBackward(webBrowser1.Url));

        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            var currentSession = session.getBackStack();
            var page = webBrowser1.Url;
            webBrowser1.Url = null;
            webBrowser1.Url = page;
            session.setBackStack(currentSession);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            try { 
                    
                    session.addBackStack(webBrowser1.Url);
               
                
                    var page = new HistoryItem();
                    page.URL = webBrowser1.Url.ToString();
                    page.Name = HistoryManager.UrlToName(webBrowser1.Url.ToString());
                    page.Date = DateTime.Now;

                    HistoryManager.AddHistory(page);
                
                
            }
            catch{ }
            
        } 

        private void addBookmark_Click(object sender, EventArgs e)
        {
            var page = new BookmarkItem();
            page.URL = webBrowser1.Url.ToString();
            page.Name = HistoryManager.UrlToName(webBrowser1.Url.ToString());
            BookmarkManager.AddBookmark(page);
        }

        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            loadingBar.Value = 100;
            progressLabel.Text = "DONE!";
            System.Threading.Thread.Sleep(1000);
            progressLabel.Visible = false;
            loadingBar.Visible = false;
            loadingBar.Value = 0;

            
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            progressLabel.Text = "Loading...";
            while (loadingBar.Value < 100)
            {
                loadingBar.Visible = true; progressLabel.Visible = true;
                loadingBar.PerformStep();
            }
            
        }
    }
}
