class MeditationPlayer {
    constructor() {
        this.audio = null;
        this.trackBase = "/audio/";
        this.extension = ".mp3"; // Change if using .wav/.ogg
    }

    play(trackName) {
        this.stop(); // Stop any current playback

        const safeName = trackName.replace(/\s+/g, '%20');
        const path = `${this.trackBase}${safeName}${this.extension}`;

        this.audio = new Audio(path);
        this.audio.loop = true;

        // Modern browsers require this
        document.body.appendChild(this.audio);

        this.audio.play()
            .then(() => console.log("Playback started"))
            .catch(e => {
                console.error("Playback error:", e);
                // Show UI error message
                alert("Couldn't play audio: " + e.message);
            });
    }

    stop() {
        if (this.audio) {
            this.audio.pause();
            this.audio.currentTime = 0;
            this.audio.remove(); // Clean up
            this.audio = null;
        }
    }
}