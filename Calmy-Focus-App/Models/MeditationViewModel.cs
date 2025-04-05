namespace Calmy_Focus_App.Models;

public class MeditationViewModel
{
    public List<MeditationSession> Sessions { get; set; }
    public List<string> AvailableTracks { get; set; }
    public MeditationSession CurrentSession { get; set; }
}