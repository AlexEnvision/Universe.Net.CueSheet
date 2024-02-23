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
using System.Text;
using Universe.NetFramework.CueSheet.Enums;

namespace Universe.NetFramework.CueSheet.Models
{
    /// <summary>
    ///     Track that contains either data or audio. It can contain Indices and comment information.
    /// </summary>
    public struct Track
    {
        #region Private Variables
        private string[] _mComments;
        // strings that don't belong or were mistyped in the global part of the cue
        private AudioFile _mDataFile;
        private string[] _mGarbage;
        private Index[] _mIndices;
        private string _mIsrc;

        private string _mPerformer;
        private Index _mPostGap;
        private Index _mPreGap;
        private string _mSongwriter;
        private string _mTitle;
        private Flags[] _mTrackFlags;
        private DataType _mTrackDataType;
        private int _mTrackNumber;
        #endregion

        #region Properties

        /// <summary>
        /// Returns/Sets Index in this track.
        /// </summary>
        /// <param name="indexnumber">Index in the track.</param>
        /// <returns>Index at indexnumber.</returns>
        public Index this[int indexnumber]
        {
            get => _mIndices[indexnumber];
            set => _mIndices[indexnumber] = value;
        }


        public string[] Comments
        {
            get => _mComments;
            set => _mComments = value;
        }


        public AudioFile DataFile
        {
            get => _mDataFile;
            set => _mDataFile = value;
        }

        /// <summary>
        /// Lines in the cue file that don't belong or have other general syntax errors.
        /// </summary>
        public string[] Garbage
        {
            get => _mGarbage;
            set => _mGarbage = value;
        }

        public Index[] Indices
        {
            get => _mIndices;
            set => _mIndices = value;
        }

        public string ISRC
        {
            get => _mIsrc;
            set => _mIsrc = value;
        }

        public string Performer
        {
            get => _mPerformer;
            set => _mPerformer = value;
        }

        public Index PostGap
        {
            get => _mPostGap;
            set => _mPostGap = value;
        }

        public Index PreGap
        {
            get => _mPreGap;
            set => _mPreGap = value;
        }

        public string Songwriter
        {
            get => _mSongwriter;
            set => _mSongwriter = value;
        }

        /// <summary>
        /// If the TITLE command appears before any TRACK commands, then the string will be encoded as the title of the entire disc.
        /// </summary>
        public string Title
        {
            get => _mTitle;
            set => _mTitle = value;
        }

        public DataType TrackDataType
        {
            get => _mTrackDataType;
            set => _mTrackDataType = value;
        }

        public Flags[] TrackFlags
        {
            get => _mTrackFlags;
            set => _mTrackFlags = value;
        }

        public int TrackNumber
        {
            get => _mTrackNumber;
            set => _mTrackNumber = value;
        }
        #endregion

        #region Contructors

        public Track(int tracknumber, string datatype)
        {
            _mTrackNumber = tracknumber;

            switch (datatype.Trim ().ToUpper ())
            {
                case "AUDIO":
                    _mTrackDataType = DataType.AUDIO;
                    break;
                case "CDG":
                    _mTrackDataType = DataType.CDG;
                    break;
                case "MODE1/2048":
                    _mTrackDataType = DataType.MODE1_2048;
                    break;
                case "MODE1/2352":
                    _mTrackDataType = DataType.MODE1_2352;
                    break;
                case "MODE2/2336":
                    _mTrackDataType = DataType.MODE2_2336;
                    break;
                case "MODE2/2352":
                    _mTrackDataType = DataType.MODE2_2352;
                    break;
                case "CDI/2336":
                    _mTrackDataType = DataType.CDI_2336;
                    break;
                case "CDI/2352":
                    _mTrackDataType = DataType.CDI_2352;
                    break;
                default:
                    _mTrackDataType = DataType.AUDIO;
                    break;
            }

            _mTrackFlags = new Flags[0];
            _mSongwriter = "";
            _mTitle = "";
            _mIsrc = "";
            _mPerformer = "";
            _mIndices = new Index[0];
            _mGarbage = new string[0];
            _mComments = new string[0];
            _mPreGap = new Index(-1, 0, 0, 0);
            _mPostGap = new Index(-1, 0, 0, 0);
            _mDataFile = new AudioFile();
        }

        public Track(int tracknumber, DataType datatype)
        {
            _mTrackNumber = tracknumber;
            _mTrackDataType = datatype;

            _mTrackFlags = new Flags[0];
            _mSongwriter = "";
            _mTitle = "";
            _mIsrc = "";
            _mPerformer = "";
            _mIndices = new Index[0];
            _mGarbage = new string[0];
            _mComments = new string[0];
            _mPreGap = new Index(-1, 0, 0, 0);
            _mPostGap = new Index(-1, 0, 0, 0);
            _mDataFile = new AudioFile();
        }
        
        #endregion

        #region Methods
        public void AddFlag(Flags flag)
        {
            //if it's not a none tag
            //and if the tags hasn't already been added
            if (flag != Flags.NONE && NewFlag(flag) == true)
            {
                _mTrackFlags = (Flags[])CueSheet.ResizeArray(_mTrackFlags, _mTrackFlags.Length + 1);
                _mTrackFlags[_mTrackFlags.Length - 1] = flag;
            }
        }

        public void AddFlag(string flag)
        {
            switch (flag.Trim().ToUpper())
            {
                case "DATA":
                    AddFlag(Flags.DATA);
                    break;
                case "DCP":
                    AddFlag(Flags.DCP);
                    break;
                case "4CH":
                    AddFlag(Flags.CH4);
                    break;
                case "PRE":
                    AddFlag(Flags.PRE);
                    break;
                case "SCMS":
                    AddFlag(Flags.SCMS);
                    break;
                default:
                    return;
            }
        }

        public void AddGarbage(string garbage)
        {
            if (garbage.Trim() != "")
            {
                _mGarbage = (string[])CueSheet.ResizeArray(_mGarbage, _mGarbage.Length + 1);
                _mGarbage[_mGarbage.Length - 1] = garbage;
            }
        }

        public void AddComment(string comment)
        {
            if (comment.Trim() != "")
            {
                _mComments = (string[])CueSheet.ResizeArray(_mComments, _mComments.Length + 1);
                _mComments[_mComments.Length - 1] = comment;
            }
        }

        public void AddIndex(int number, int minutes, int seconds, int frames)
        {
            _mIndices = (Index[])CueSheet.ResizeArray(_mIndices, _mIndices.Length + 1);

            _mIndices[_mIndices.Length - 1] = new Index(number, minutes, seconds, frames);
        }

        public void RemoveIndex(int indexIndex)
        {
            for (int i = indexIndex; i < _mIndices.Length - 1; i++)
            {
                _mIndices[i] = _mIndices[i + 1];
            }
            _mIndices = (Index[])CueSheet.ResizeArray(_mIndices, _mIndices.Length - 1);
        }

        /// <summary>
        /// Checks if the flag is indeed new in this track.
        /// </summary>
        /// <param name="flag">The new flag to be added to the track.</param>
        /// <returns>True if this flag doesn't already exist.</returns>
        private bool NewFlag(Flags newFlag)
        {
            foreach (Flags flag in _mTrackFlags)
            {
                if (flag == newFlag)
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            //write file
            if (_mDataFile.Filename != null && _mDataFile.Filename.Trim() != "")
            {
                output.Append("FILE \"" + _mDataFile.Filename.Trim() + "\" " + _mDataFile.Filetype.ToString() + Environment.NewLine);
            }

            output.Append("  TRACK " + _mTrackNumber.ToString().PadLeft(2, '0') + " " + _mTrackDataType.ToString().Replace('_', '/'));

            //write comments
            foreach (string comment in _mComments)
            {
                output.Append(Environment.NewLine + "    REM " + comment);
            }

            if (_mPerformer.Trim() != "")
            {
                output.Append(Environment.NewLine + "    PERFORMER \"" + _mPerformer + "\"");
            }

            if (_mSongwriter.Trim() != "")
            {
                output.Append(Environment.NewLine + "    SONGWRITER \"" + _mSongwriter + "\"");
            }

            if (_mTitle.Trim() != "")
            {
                output.Append(Environment.NewLine + "    TITLE \"" + _mTitle + "\"");
            }

            //write flags
            if (_mTrackFlags.Length > 0)
            {
                output.Append(Environment.NewLine + "    FLAGS");
            }

            foreach (Flags flag in _mTrackFlags)
            {
                output.Append(" " + flag.ToString().Replace("CH4", "4CH"));
            }

            //write isrc
            if (_mIsrc.Trim() != "")
            {
                output.Append(Environment.NewLine + "    ISRC " + _mIsrc.Trim());
            }

            //write pregap
            if (_mPreGap.Number != -1)
            {
                output.Append(Environment.NewLine + "    PREGAP " + _mPreGap.Minutes.ToString().PadLeft(2, '0') + ":" + _mPreGap.Seconds.ToString().PadLeft(2, '0') + ":" + _mPreGap.Frames.ToString().PadLeft(2, '0'));
            }

            //write Indices
            for (int j = 0; j < _mIndices.Length; j++)
            {
                output.Append(Environment.NewLine + "    INDEX " + this[j].Number.ToString().PadLeft(2, '0') + " " + this[j].Minutes.ToString().PadLeft(2, '0') + ":" + this[j].Seconds.ToString().PadLeft(2, '0') + ":" + this[j].Frames.ToString().PadLeft(2, '0'));
            }

            //write postgap
            if (_mPostGap.Number != -1)
            {
                output.Append(Environment.NewLine + "    POSTGAP " + _mPostGap.Minutes.ToString().PadLeft(2, '0') + ":" + _mPostGap.Seconds.ToString().PadLeft(2, '0') + ":" + _mPostGap.Frames.ToString().PadLeft(2, '0'));
            }

            return output.ToString();
        }

        #endregion Methods
    }
}