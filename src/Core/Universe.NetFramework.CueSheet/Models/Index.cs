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

namespace Universe.NetFramework.CueSheet.Models
{
    /// <summary>
    /// This command is used to specify indexes (or subindexes) within a track.
    /// Syntax:
    ///  INDEX [number] [mm:ss:ff]
    /// </summary>
    public struct Index
    {
        //0-99
        int _mNumber;

        int _mMinutes;
        int _mSeconds;
        int _mFrames;

        /// <summary>
        /// Index number (0-99)
        /// </summary>
        public int Number
        {
            get => _mNumber;
            set
            {
                if (value > 99)
                {
                    _mNumber = 99;
                }
                else if (value < 0)
                {
                    _mNumber = 0;
                }
                else
                {
                    _mNumber = value;
                }
            }
        }

        /// <summary>
        /// Possible values: 0-99
        /// </summary>
        public int Minutes
        {
            get => _mMinutes;
            set
            {
                if (value > 99)
                {
                    _mMinutes = 99;
                }
                else if (value < 0)
                {
                    _mMinutes = 0;
                }
                else
                {
                    _mMinutes = value;
                }
            }
        }

        /// <summary>
        /// Possible values: 0-59
        /// There are 60 seconds/minute
        /// </summary>
        public int Seconds
        {
            get => _mSeconds;
            set
            {
                if (value >= 60)
                {
                    _mSeconds = 59;
                }
                else if (value < 0)
                {
                    _mSeconds = 0;
                }
                else
                {
                    _mSeconds = value;
                }
            }
        }

        /// <summary>
        /// Possible values: 0-74
        /// There are 75 frames/second
        /// </summary>
        public int Frames
        {
            get => _mFrames;
            set
            {
                if (value >= 75)
                {
                    _mFrames = 74;
                }
                else if (value < 0)
                {
                    _mFrames = 0;
                }
                else
                {
                    _mFrames = value;
                }
            }
        }

        /// <summary>
        /// The Index of a track.
        /// </summary>
        /// <param name="number">Index number 0-99</param>
        /// <param name="minutes">Minutes (0-99)</param>
        /// <param name="seconds">Seconds (0-59)</param>
        /// <param name="frames">Frames (0-74)</param>
        public Index(int number, int minutes, int seconds, int frames)
        {
            _mNumber = number;

            _mMinutes = minutes;
            _mSeconds = seconds;
            _mFrames = frames;
        }
    }
}