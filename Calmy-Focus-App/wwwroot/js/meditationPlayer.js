class MeditationPlayer {
    constructor() {
        this.audio = new Audio();
        this.tracks = {
            "Ocean Waves": "/audio/ocean-waves.mp3",
            "Forest Rain": "/audio/forest-rain.mp3",
            // Add other tracks
        };
    }

    play(trackName) {
        this.audio.src = this.tracks[trackName];
        this.audio.loop = true;
        this.audio.play();
    }

    stop() {
        this.audio.pause();
        this.audio.currentTime = 0;
    }
}