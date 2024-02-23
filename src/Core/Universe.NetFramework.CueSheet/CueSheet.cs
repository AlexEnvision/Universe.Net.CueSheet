//  ╔═════════════════════════════════════════════════════════════════════════════════╗
//  ║                                                                                 ║
//  ║   Copyright 2024 Universe.Net.CueSheet                                          ║
//  ║                                                                                 ║
//  ║   Licensed under the Apache License, Version 2.0 (the "License");               ║
//  ║   you may not use this file except in compliance with the License.              ║
//  ║   You may obtain a copy of the License at                                       ║
//  ║                                                                                 ║
//  ║       http://www.apache.org/licenses/LICENSE-2.0                                ║
//  ║                                                                                 ║
//  ║   Unless required by applicable law or agreed to in writing, software           ║
//  ║   distributed under the License is distributed on an "AS IS" BASIS,             ║
//  ║   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.      ║
//  ║   See the License for the specific language governing permissions and           ║
//  ║   limitations under the License.                                                ║
//  ║                                                                                 ║
//  ║                                                                                 ║
//  ║   Copyright 2024 Universe.Net.CueSheet                                          ║
//  ║                                                                                 ║
//  ║   Лицензировано согласно Лицензии Apache, Версия 2.0 ("Лицензия");              ║
//  ║   вы можете использовать этот файл только в соответствии с Лицензией.           ║
//  ║   Вы можете найти копию Лицензии по адресу                                      ║
//  ║                                                                                 ║
//  ║       http://www.apache.org/licenses/LICENSE-2.0.                               ║
//  ║                                                                                 ║
//  ║   За исключением случаев, когда это регламентировано существующим               ║
//  ║   законодательством или если это не оговорено в письменном соглашении,          ║
//  ║   программное обеспечение распространяемое на условиях данной Лицензии,         ║
//  ║   предоставляется "КАК ЕСТЬ" и любые явные или неявные ГАРАНТИИ ОТВЕРГАЮТСЯ.    ║
//  ║   Информацию об основных правах и ограничениях,                                 ║
//  ║   применяемых к определенному языку согласно Лицензии,                          ║
//  ║   вы можете найти в данной Лицензии.                                            ║
//  ║                                                                                 ║
//  ╚═════════════════════════════════════════════════════════════════════════════════╝

using System;
using System.IO;
using System.Text;
using Universe.NetFramework.CueSheet.Enums;
using Universe.NetFramework.CueSheet.Models;

namespace Universe.NetFramework.CueSheet
{
    /// <summary>
    ///     A CueSheet class used to create,
    ///     open, edit, and save cuesheets.
    /// </summary>
    public class CueSheet
    {
        #region Private Variables
        string[] _cueLines;

        private string _mCatalog = "";
        private string _mCdTextFile = "";
        private string[] _mComments = new string[0];

        // strings that don't belong or were mistyped in the global part of the cue
        private string[] _mGarbage = new string[0];
        private string _mPerformer = "";
        private string _mSongwriter= "";
        private string _mTitle="";
        private Track[] _mTracks = new Track[0];

        #endregion

        #region Properties


        /// <summary>
        ///     Returns/Sets track in this cuefile.
        /// </summary>
        /// <param name="tracknumber">
        ///     The track in this cuefile.
        /// </param>
        /// <returns>
        ///     Track at the tracknumber.
        /// </returns>
        public Track this[int tracknumber]
        {
            get => _mTracks[tracknumber];
            set => _mTracks[tracknumber] = value;
        }


        /// <summary>
        ///     The catalog number must be 13 digits long and is encoded according to UPC/EAN rules.
        ///     Example: CATALOG 1234567890123
        /// </summary>
        public string Catalog
        {
            get => _mCatalog;
            set => _mCatalog = value;
        }

        /// <summary>
        ///     This command is used to specify the name of the file that contains the encoded CD-TEXT information for the disc. This command is only used with files that were either created with the graphical CD-TEXT editor or generated automatically by the software when copying a CD-TEXT enhanced disc.
        /// </summary>
        public string CDTextFile
        {
            get => _mCdTextFile;
            set => _mCdTextFile = value;
        }

        /// <summary>
        ///     This command is used to put comments in your CUE SHEET file.
        /// </summary>
        public string[] Comments
        {
            get => _mComments;
            set => _mComments = value;
        }

        /// <summary>
        ///     Lines in the cue file that don't belong or have other general syntax errors.
        /// </summary>
        public string[] Garbage => _mGarbage;

        /// <summary>
        ///     This command is used to specify the name of a perfomer for a CD-TEXT enhanced disc.
        /// </summary>
        public string Performer
        {
            get => _mPerformer;
            set => _mPerformer = value;
        }

        /// <summary>
        ///     This command is used to specify the name of a songwriter for a CD-TEXT enhanced disc.
        /// </summary>
        public string Songwriter
        {
            get => _mSongwriter;
            set => _mSongwriter = value;
        }

        /// <summary>
        ///     The title of the entire disc as a whole.
        /// </summary>
        public string Title
        {
            get => _mTitle;
            set => _mTitle = value;
        }

        /// <summary>
        ///     The array of tracks on the cuesheet.
        /// </summary>
        public Track[] Tracks
        {
            get => _mTracks;
            set => _mTracks = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Create a cue sheet from scratch.
        /// </summary>
        public CueSheet()
        { }

        /// <summary>
        ///     Parse a cue sheet string.
        /// </summary>
        /// <param name="cueString">
        ///     A string containing the cue sheet data.
        /// </param>
        /// <param name="lineDelims">
        ///     Line delimeters; set to "(char[])null" for default delimeters.
        /// </param>
        public CueSheet(string cueString, char[] lineDelims)
        {
            if (lineDelims == null)
            {
                lineDelims = new char[] { '\n' };
            }

            _cueLines = cueString.Split(lineDelims);
            RemoveEmptyLines(ref _cueLines);
            ParseCue(_cueLines);
        }

        /// <summary>
        ///     Parses a cue sheet file.
        /// </summary>
        /// <param name="cuefilename">
        ///     The filename for the cue sheet to open.
        /// </param>
        public CueSheet(string cuefilename)
        {
            ReadCueSheet(cuefilename, Encoding.Default);
        }

        /// <summary>
        ///     Parses a cue sheet file.
        /// </summary>
        /// <param name="cuefilename">
        ///     The filename for the cue sheet to open.
        /// </param>
        /// <param name="encoding">
        ///     The encoding used to open the file.
        /// </param>
        public CueSheet(string cuefilename, Encoding encoding)
        {
            ReadCueSheet(cuefilename, encoding);
        }

        private void ReadCueSheet(string filename, Encoding encoding)
        {
            // array of delimiters to split the sentence with
            char[] delimiters = new char[] { '\n' };
            
            // read in the full cue file
            TextReader tr = new StreamReader(filename, encoding);
            //read in file
            _cueLines = tr.ReadToEnd().Split(delimiters);

            // close the stream
            tr.Close();

            RemoveEmptyLines(ref _cueLines);

            ParseCue(_cueLines);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Removes any empty lines, elimating possible trouble.
        /// </summary>
        /// <param name="file"></param>
        private void RemoveEmptyLines(ref string[] file)
        {
            int itemsRemoved = 0;

            for (int i = 0; i < file.Length; i++)
            {
                if (file[i].Trim() != "")
                {
                    file[i - itemsRemoved] = file[i];
                }
                else if (file[i].Trim() == "")
                {
                    itemsRemoved++;
                }
            }

            if (itemsRemoved > 0)
            {
                file = (string[])ResizeArray(file, file.Length - itemsRemoved);
            }
        }

        private void ParseCue(string[] file)
        {
            //-1 means still global, 
            //all others are track specific
            int trackOn = -1;
            AudioFile currentFile = new AudioFile();

            for (int i = 0; i < file.Length; i++)
            {
                file[i] = file[i].Trim();

                switch (file[i].Substring(0, file[i].IndexOf(' ')).ToUpper())
                {
                    case "CATALOG":
                        ParseString(file[i], trackOn);
                        break;
                    case "CDTEXTFILE":
                        ParseString(file[i], trackOn);
                        break;
                    case "FILE":
                        currentFile = ParseFile(file[i], trackOn);
                        break;
                    case "FLAGS":
                        ParseFlags(file[i], trackOn);
                        break;
                    case "INDEX":
                        ParseIndex(file[i], trackOn);
                        break;
                    case "ISRC":
                        ParseString(file[i], trackOn);
                        break;
                    case "PERFORMER":
                        ParseString(file[i], trackOn);
                        break;
                    case "POSTGAP":
                        ParseIndex(file[i], trackOn);
                        break;
                    case "PREGAP":
                        ParseIndex(file[i], trackOn);
                        break;
                    case "REM":
                        ParseComment(file[i], trackOn);
                        break;
                    case "SONGWRITER":
                        ParseString(file[i], trackOn);
                        break;
                    case "TITLE":
                        ParseString(file[i], trackOn);
                        break;
                    case "TRACK":
                        trackOn++;
                        ParseTrack(file[i], trackOn);
                        if (currentFile.Filename != "") //if there's a file
                        {
                            _mTracks[trackOn].DataFile = currentFile;
                            currentFile = new AudioFile();
                        }
                        break;
                    default:
                        ParseGarbage(file[i], trackOn);
                        //save discarded junk and place string[] with track it was found in
                        break;
                }
            }

        }

        private void ParseComment(string line, int trackOn)
        {
            //remove "REM" (we know the line has already been .Trim()'ed)
            line = line.Substring(line.IndexOf(' '), line.Length - line.IndexOf(' ')).Trim();

            if (trackOn == -1)
            {
                if (line.Trim() != "")
                {
                    _mComments = (string[])ResizeArray(_mComments, _mComments.Length + 1);
                    _mComments[_mComments.Length - 1] = line;
                }
            }
            else
            {
                _mTracks[trackOn].AddComment(line);
            }
        }

        private AudioFile ParseFile(string line, int trackOn)
        {
            string fileType;

            line = line.Substring(line.IndexOf(' '), line.Length - line.IndexOf(' ')).Trim();

            fileType = line.Substring(line.LastIndexOf(' '), line.Length - line.LastIndexOf(' ')).Trim();

            line = line.Substring(0, line.LastIndexOf(' ')).Trim();

            //if quotes around it, remove them.
            if (line[0] == '"')
            {
                line = line.Substring(1, line.LastIndexOf('"') - 1);
            }

            return new AudioFile(line, fileType);
        }

        private void ParseFlags(string line, int trackOn)
        {
            string temp;

            if (trackOn != -1)
            {
                line = line.Trim();
                if (line != "")
                {
                    try
                    {
                        temp = line.Substring(0, line.IndexOf(' ')).ToUpper();
                    }
                    catch (Exception)
                    {
                        temp = line.ToUpper();
                        
                    }

                    switch (temp)
                    {
                        case "FLAGS":
                            _mTracks[trackOn].AddFlag(temp);
                            break;
                        case "DATA":
                            _mTracks[trackOn].AddFlag(temp);
                            break;
                        case "DCP":
                            _mTracks[trackOn].AddFlag(temp);
                            break;
                        case "4CH":
                            _mTracks[trackOn].AddFlag(temp);
                            break;
                        case "PRE":
                            _mTracks[trackOn].AddFlag(temp);
                            break;
                        case "SCMS":
                            _mTracks[trackOn].AddFlag(temp);
                            break;
                        default:
                            break;
                    }

                    //processing for a case when there isn't any more spaces
                    //i.e. avoiding the "index cannot be less than zero" error
                    //when calling line.IndexOf(' ')
                    try
                    {
                        temp = line.Substring(line.IndexOf(' '), line.Length - line.IndexOf(' '));
                    }
                    catch (Exception)
                    {
                        temp = line.Substring(0, line.Length);
                    }

                    //if the flag hasn't already been processed
                    if (temp.ToUpper().Trim() != line.ToUpper().Trim())
                    {
                        ParseFlags(temp, trackOn);
                    }
                }
            }
        }

        private void ParseGarbage(string line, int trackOn)
        {
            if (trackOn == -1)
            {
                if (line.Trim() != "")
                {
                    _mGarbage = (string[])ResizeArray(_mGarbage, _mGarbage.Length + 1);
                    _mGarbage[_mGarbage.Length - 1] = line;
                }
            }
            else
            {
                _mTracks[trackOn].AddGarbage(line);
            }
        }

        private void ParseIndex(string line, int trackOn)
        {
            string indexType;
            string tempString;

            int number = 0;
            int minutes;
            int seconds;
            int frames;

            indexType = line.Substring(0, line.IndexOf(' ')).ToUpper();

            tempString = line.Substring(line.IndexOf(' '), line.Length - line.IndexOf(' ')).Trim();

            if (indexType == "INDEX")
            {
                //read the index number
                number = Convert.ToInt32(tempString.Substring(0, tempString.IndexOf(' ')));
                tempString = tempString.Substring(tempString.IndexOf(' '), tempString.Length - tempString.IndexOf(' ')).Trim();
            }

            //extract the minutes, seconds, and frames
            minutes = Convert.ToInt32(tempString.Substring(0, tempString.IndexOf(':')));
            seconds = Convert.ToInt32(tempString.Substring(tempString.IndexOf(':') + 1, tempString.LastIndexOf(':') - tempString.IndexOf(':') - 1));
            frames = Convert.ToInt32(tempString.Substring(tempString.LastIndexOf(':') + 1, tempString.Length - tempString.LastIndexOf(':') - 1));

            if (indexType == "INDEX")
            {
                _mTracks[trackOn].AddIndex(number, minutes, seconds, frames);
            }
            else if (indexType == "PREGAP")
            {
                _mTracks[trackOn].PreGap = new Index(0, minutes, seconds, frames);
            }
            else if (indexType == "POSTGAP")
            {
                _mTracks[trackOn].PostGap = new Index(0, minutes, seconds, frames);
            }
        }

        private void ParseString(string line, int trackOn)
        {
            string category = line.Substring(0, line.IndexOf(' ')).ToUpper();

            line = line.Substring(line.IndexOf(' '), line.Length - line.IndexOf(' ')).Trim();

            //get rid of the quotes
            if (line[0] == '"')
            {
                line = line.Substring(1, line.LastIndexOf('"') - 1);
            }

            switch (category)
            {
                case "CATALOG":
                    if (trackOn == -1)
                    {
                        this._mCatalog = line;
                    }
                    break;
                case "CDTEXTFILE":
                    if (trackOn == -1)
                    {
                        this._mCdTextFile = line;
                    }
                    break;
                case "ISRC":
                    if (trackOn != -1)
                    {
                        _mTracks[trackOn].ISRC = line;
                    }
                    break;
                case "PERFORMER":
                    if (trackOn == -1)
                    {
                        this._mPerformer = line;
                    }
                    else
                    {
                        _mTracks[trackOn].Performer = line;
                    }
                    break;
               case "SONGWRITER":
                   if (trackOn == -1)
                   {
                       this._mSongwriter = line;
                   }
                   else
                   {
                       _mTracks[trackOn].Songwriter = line;
                   }
                    break;
                case "TITLE":
                    if (trackOn == -1)
                    {
                        this._mTitle = line;
                    }
                    else
                    {
                        _mTracks[trackOn].Title = line;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///     Parses the TRACK command. 
        /// </summary>
        /// <param name="line">
        ///     The line in the cue file that contains the TRACK command.
        /// </param>
        /// <param name="trackOn">
        ///     The track currently processing.
        /// </param>
        private void ParseTrack(string line, int trackOn)
        {
            string tempString;
            int trackNumber;

            tempString = line.Substring(line.IndexOf(' '), line.Length - line.IndexOf(' ')).Trim();

            try
            {
                trackNumber = Convert.ToInt32(tempString.Substring(0, tempString.IndexOf(' ')));
            }
            catch (Exception)
            { throw; }

            //find the data type.
            tempString = tempString.Substring(tempString.IndexOf(' '), tempString.Length - tempString.IndexOf(' ')).Trim();

            AddTrack(trackNumber, tempString);
        }
        
        /// <summary>
        ///     Reallocates an array with a new size, and copies the contents
        ///     of the old array to the new array.
        /// </summary>
        /// <param name="oldArray">
        ///     The old array, to be reallocated.
        /// </param>
        /// <param name="newSize">
        ///     The new array size.
        /// </param>
        /// <returns>
        ///     A new array with the same contents.
        /// </returns>
        /// <remarks>
        ///     Useage: int[] a = {1,2,3}; a = (int[])ResizeArray(a,5);
        /// </remarks>
        public static System.Array ResizeArray(System.Array oldArray, int newSize)
        {
            int oldSize = oldArray.Length;
            System.Type elementType = oldArray.GetType().GetElementType();
            System.Array newArray = System.Array.CreateInstance(elementType, newSize);
            int preserveLength = System.Math.Min(oldSize, newSize);
            if (preserveLength > 0)
                System.Array.Copy(oldArray, newArray, preserveLength);
            return newArray;
        }

        /// <summary>
        ///     Add a track to the current cuesheet.
        /// </summary>
        /// <param name="tracknumber">The number of the said track.</param>
        /// <param name="datatype">The datatype of the track.</param>
        private void AddTrack(int tracknumber, string datatype)
        {
            _mTracks = (Track[])ResizeArray(_mTracks, _mTracks.Length + 1);
            _mTracks[_mTracks.Length - 1] = new Track(tracknumber, datatype);
        }

        /// <summary>
        ///     Add a track to the current cuesheet
        /// </summary>
        /// <param name="title">The title of the track.</param>
        /// <param name="performer">The performer of this track.</param>
        public void AddTrack(string title, string performer)
        {
            _mTracks = (Track[])ResizeArray(_mTracks, _mTracks.Length + 1);
            _mTracks[_mTracks.Length - 1] = new Track(_mTracks.Length, "");

            _mTracks[_mTracks.Length - 1].Performer = performer;
            _mTracks[_mTracks.Length - 1].Title = title;
        }


        public void AddTrack(string title, string performer, string filename, FileType fType)
        {
            _mTracks = (Track[])ResizeArray(_mTracks, _mTracks.Length + 1);
            _mTracks[_mTracks.Length - 1] = new Track(_mTracks.Length, "");

            _mTracks[_mTracks.Length - 1].Performer = performer;
            _mTracks[_mTracks.Length - 1].Title = title;
            _mTracks[_mTracks.Length - 1].DataFile = new AudioFile (filename, fType);
        }

        /// <summary>
        ///     Add a track to the current cuesheet
        /// </summary>
        /// <param name="title">The title of the track.</param>
        /// <param name="performer">The performer of this track.</param>
        /// <param name="datatype">The datatype for the track (typically DataType.Audio)</param>
        public void AddTrack(string title, string performer, DataType datatype)
        {
            _mTracks = (Track[])ResizeArray(_mTracks, _mTracks.Length + 1);
            _mTracks[_mTracks.Length - 1] = new Track(_mTracks.Length, datatype);

            _mTracks[_mTracks.Length - 1].Performer = performer;
            _mTracks[_mTracks.Length - 1].Title = title;
        }

        /// <summary>
        ///     Add a track to the current cuesheet
        /// </summary>
        /// <param name="track">Track object to add to the cuesheet.</param>
        public void AddTrack(Track track)
        {
            _mTracks = (Track[])ResizeArray(_mTracks, _mTracks.Length + 1);
            _mTracks[_mTracks.Length - 1] = track;
        }

        /// <summary>
        ///     Remove a track from the cuesheet.
        /// </summary>
        /// <param name="trackIndex">The index of the track you wish to remove.</param>
        public void RemoveTrack(int trackIndex)
        {
            for (int i = trackIndex; i < _mTracks.Length - 1; i++)
            {
                _mTracks[i] = _mTracks[i + 1];
            }
            _mTracks = (Track[])ResizeArray(_mTracks, _mTracks.Length - 1);
        }

        /// <summary>
        ///     Add index information to an existing track.
        /// </summary>
        /// <param name="trackIndex">The array index number of track to be modified</param>
        /// <param name="indexNum">The index number of the new index</param>
        /// <param name="minutes">The minute value of the new index</param>
        /// <param name="seconds">The seconds value of the new index</param>
        /// <param name="frames">The frames value of the new index</param>
        public void AddIndex(int trackIndex, int indexNum, int minutes, int seconds, int frames)
        {
            _mTracks[trackIndex].AddIndex(indexNum, minutes, seconds, frames);
        }

        /// <summary>
        ///     Remove an index from a track.
        /// </summary>
        /// <param name="trackIndex">The array-index of the track.</param>
        /// <param name="indexIndex">The index of the Index you wish to remove.</param>
        public void RemoveIndex(int trackIndex, int indexIndex)
        {
            //Note it is the index of the Index you want to delete, 
            //which may or may not correspond to the number of the index.
            _mTracks[trackIndex].RemoveIndex(indexIndex);
        }

        /// <summary>
        /// Save the cue sheet file to specified location.
        /// </summary>
        /// <param name="filename">Filename of destination cue sheet file.</param>
        public void SaveCue(string filename)
        {
            SaveCue(filename, Encoding.Default);
        }

        /// <summary>
        ///     Save the cue sheet file to specified location.
        /// </summary>
        /// <param name="filename">Filename of destination cue sheet file.</param>
        /// <param name="encoding">The encoding used to save the file.</param>
        public void SaveCue(string filename, Encoding encoding)
        {
            TextWriter tw = new StreamWriter(filename, false, encoding);

            tw.WriteLine(this.ToString());

            //close the writer stream
            tw.Close();
        }

        /// <summary>
        ///     Method to output the cuesheet into a single formatted string.
        /// </summary>
        /// <returns>The entire cuesheet formatted to specification.</returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            foreach (string comment in _mComments)
            {
                output.Append("REM " + comment + Environment.NewLine);
            }

            if (_mCatalog.Trim() != "")
            {
                output.Append("CATALOG " + _mCatalog + Environment.NewLine);
            }

            if (_mPerformer.Trim() != "")
            {
                output.Append("PERFORMER \"" + _mPerformer + "\"" + Environment.NewLine);
            }

            if (_mSongwriter.Trim() != "")
            {
                output.Append("SONGWRITER \"" + _mSongwriter + "\"" + Environment.NewLine);
            }

            if (_mTitle.Trim() != "")
            {
                output.Append("TITLE \"" + _mTitle + "\"" + Environment.NewLine);
            }

            if (_mCdTextFile.Trim() != "")
            {
                output.Append("CDTEXTFILE \"" + _mCdTextFile.Trim() + "\"" + Environment.NewLine);
            }

            for (int i = 0; i < _mTracks.Length; i++)
            {
                output.Append(_mTracks[i].ToString());

                if (i != _mTracks.Length - 1)
                {
                    //add line break for each track except last
                    output.Append(Environment.NewLine);
                }
            }

            return output.ToString();
        }

        #endregion

        //TODO: Need to fix calculation bugs, if they will be - 
        //TODO: currently generates erroneous IDs.
        #region CalculateDiscIDs
        //For complete CDDB/freedb discID calculation, see:
        //http://www.freedb.org/modules.php?name=Sections&sop=viewarticle&artid=6

        public string CalculateCDDBdiscID()
        {
            int i, t = 0, n = 0;

            /* For backward compatibility this algorithm must not change */

            i = 0;

            while (i < _mTracks.Length)
            {
                n = n + cddb_sum((lastTrackIndex(_mTracks[i]).Minutes * 60) + lastTrackIndex(_mTracks[i]).Seconds);
                i++;
            }

            Console.WriteLine(n.ToString());

            t = ((lastTrackIndex(_mTracks[_mTracks.Length - 1]).Minutes * 60) + lastTrackIndex(_mTracks[_mTracks.Length - 1]).Seconds) -
                ((lastTrackIndex(_mTracks[0]).Minutes * 60) + lastTrackIndex(_mTracks[0]).Seconds);

            ulong lDiscId = (((uint)n % 0xff) << 24 | (uint)t << 8 | (uint)_mTracks.Length);
            return String.Format("{0:x8}", lDiscId);
        }

        private Index lastTrackIndex(Track track)
        {
            return track.Indices[track.Indices.Length - 1];
        }

        private int cddb_sum(int n)
        {
            int ret;

            /* For backward compatibility this algorithm must not change */

            ret = 0;

            while (n > 0)
            {
                ret = ret + (n % 10);
                n = n / 10;
            }

            return (ret);
        }

        #endregion CalculateDiscIDS
    }
}