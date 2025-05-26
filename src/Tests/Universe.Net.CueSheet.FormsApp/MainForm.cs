using Universe.Algorithm.MultiThreading;
using Universe.Diagnostic.Logger;
using Universe.Windows.Forms.Controls;

namespace Universe.Net.CueSheet.FormsApp
{
    /// <summary>
    /// <author>Alex Universe</author>
    /// </summary>
    public partial class MainForm : Form
    {
        private ThreadMachine _threadMachine;

        private readonly EventLogger _log;

        public MainForm()
        {
            InitializeComponent();

            _log = new EventLogger();

            _log.LogInfo += e =>
            {
                if (e.AllowReport)
                {
                    var currentDate = DateTime.Now;
                    var message = $"[{currentDate}] {e.Message}{Environment.NewLine}";
                    
                    this.SafeCall(() => tbLog.AppendText($"[{currentDate:dd:MM:yyyy} {currentDate:hh:mm}]:{message}{Environment.NewLine}"));
                }
            };

            _log.LogError += e =>
            {
                if (e.AllowReport)
                {
                    var currentDate = DateTime.Now;
                    var message = $"[{currentDate}] �� ����� ���������� �������� ��������� ������. ����� ������: {e.Message}.{Environment.NewLine} ����������� �����: {e.Ex.StackTrace}{Environment.NewLine}";

                    this.SafeCall(() => 
                        tbLog.AppendText($"[{currentDate:dd:MM:yyyy} {currentDate:hh:mm}]:{message}{Environment.NewLine}" +
                                         $" StackTrace: {e.Ex.Message}{Environment.NewLine}"));
                }
            };
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = @"CUE Sheet|*.cue";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var filename = dialog.FileName;
                    var cuesheet = OpenExistingFile(filename);

                    var trackNames = cuesheet.Tracks.Select(x => x.Title).ToList();
                    lbTracks.DataSource = trackNames;
                }
            }
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = @"CUE Sheet|*.cue";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var filename = dialog.FileName;
                    var cuesheet = OpenExistingFile(filename);

                    var trackNames = cuesheet.Tracks.Select(x => x.Title).ToList();
                    lbTracks.DataSource = trackNames;
                }
            }
        }

        /// <summary>
        /// Open an manipulate an existing cuesheet.
        /// </summary>
        public CueSheet OpenExistingFile(string filename)
        {
            //open a cuesheet from file with the default encoding.
            CueSheet cue = new CueSheet(filename);

            //print out the title from the global section of the cue file
            _log.Info(cue.Title);

            //print out the performer from the global section of the cue file
            _log.Info(cue.Performer);

            //Show how many track there are
            _log.Info("There are " + cue.Tracks.Length.ToString() + "tracks in this cue file.");

            //Write out the first track
            _log.Info(cue[0].ToString());

            _log.Info("------ Start CueSheet -------");
            _log.Info(cue.ToString());
            _log.Info("------  End CueSheet  -------");

            //print out the title of the second track
            _log.Info(cue[1].Title);

            //print out the Minutes, seconds, and frames of the first index of the second track.
            _log.Info(cue[1][0].Minutes.ToString());
            _log.Info(cue[1][0].Seconds.ToString());
            _log.Info(cue[1][0].Frames.ToString());

            //Print out the image filename
            _log.Info("Data file location: " + cue[0].DataFile.Filename);

            return cue;

            //change the title of the cuesheet
            //cue.Title = "Great Music";

            //Manipulate the first track
            //Track tempTrack = cue[0];//create a tempTrack to change info
            //tempTrack.AddFlag(Flags.CH4);//add a new flag
            //tempTrack.Title = "Wonderful track #1";//change the title
            //cue[0] = tempTrack;//Set the 1st track with the newly manipulated track.

            //cue.SaveCue("newcue.cue");
        }
    }
}
