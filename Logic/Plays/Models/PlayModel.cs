using System;

namespace TheatreApi.Logic.Plays
{
    public class PlayModel
    {
        public string Title {get; set;}
        public DateTime DateTime {get; set;}
        public string Actors {get; set;}
        public string Description {get; set;}
        public string AuditoriumName {get; set;}
    }
}