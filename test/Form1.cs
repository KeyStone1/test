using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imatest.IT;
using System.IO;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace test
{
    public partial class MainForm : Form
    {
        //emgu
        private Capture _capture = null;
        private bool _captureInProgress;

        //imatest
        bool bIm = false;
        public const string EXAMPLE_IMAGE = "colorcheck_example.jpg";

        public MainForm()
        {
            InitializeComponent();
            //Emgu Connect
            try
            {
                _capture = new Capture();
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }


            
                
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            using (Library imlib = new Library())
            {
                bIm = true;
                tbMsg.AppendText("imlib initilized.\n");
                DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
                string rootDir = currentDirectory.Parent.Parent.Parent.FullName;
                string imagePath = Path.Combine(rootDir, EXAMPLE_IMAGE);

                string result = imlib.ColorCheck.JSON(rootDir, imagePath, OperationMode.Standard);
                tbMsg.AppendText(result);
            }
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> frame = _capture.RetrieveBgrFrame();
            imbxCapture.Image = frame;


        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_capture!=null)
            {
                if (_captureInProgress)
                {
                    btnStart.Text = "Start";
                    _capture.Pause();
                }
                else
                {
                    btnStart.Text="Stop";
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }

        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }
        
    }
}
