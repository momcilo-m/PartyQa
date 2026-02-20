import { serverUrl } from "../../config.js";
import { Party } from "./party.js";


export class MyPartiesPage {

    constructor() {
        this.container = document.body.querySelector(".container");
        this.parties = [];
    }

    async draw() {
        const myPartiesRequest = await fetch (serverUrl + "/Party/my-parties/" + localStorage.getItem("id"));

        if (!myPartiesRequest.ok) return;

        this.parties = await myPartiesRequest.json();

        this.parties.forEach(p => {
            const party = new Party(p);
            party.draw(this.container);
        });
    }
}

const myPartiesPage = new MyPartiesPage();
myPartiesPage.draw();