import { serverUrl } from "../../config.js";
import { Party } from "./party.js";


export class PartiesPage {
    constructor() {
        this.container = document.body.querySelector(".container");
        this.parties = [];
    }

    async draw() {  
        console.log(localStorage.getItem("id"));
        const partiesRequest = await fetch (serverUrl + "/Party/available-parties/");

        if (!partiesRequest.ok) return;

        this.parties = await partiesRequest.json();
        console.log(this.parties)
        this.parties.forEach(p => {
            const party = new Party(p);
            party.draw(this.container);
        });
    }
}

const partiesPage = new PartiesPage();
await partiesPage.draw();