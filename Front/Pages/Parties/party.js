import { serverUrl } from "../../config.js";
import { prefix64Encoded } from "../../constants.js";


export class Party {
    constructor(party) {
        this.container = null;
        this.party = party;
    }
    
    draw(container) {

        const card = document.createElement("sl-card");
        this.container = card;
        card.classList.add("card-overview");
        container.appendChild(card);

        const cardImg = document.createElement("img");
        cardImg.classList.add("card-image");
        cardImg.setAttribute("slot", "image");
        cardImg.setAttribute("src", prefix64Encoded + this.party.image);
        cardImg.setAttribute("alt", "party image");
        card.appendChild(cardImg);

        const partyNameLab = document.createElement("strong");
        partyNameLab.textContent = this.party.name;
        card.appendChild(partyNameLab);
        card.appendChild(document.createElement("br"));
        card.appendChild(document.createElement("br"));

        const partyCityLab = document.createElement("label");
        partyCityLab.textContent = "City: " + this.party.city;
        card.appendChild(partyCityLab);
        card.appendChild(document.createElement("br"));

        const partyAddressLab = document.createElement("label");
        partyAddressLab.textContent = "Address: " + this.party.address;
        card.appendChild(partyAddressLab);

        const footerDiv = document.createElement("div");
        footerDiv.classList.add("footer-div");
        footerDiv.setAttribute("slot", "footer");
        card.appendChild(footerDiv);

        const attendPartyButton = document.createElement("sl-button");
        attendPartyButton.textContent = "Attend Party";
        attendPartyButton.addEventListener("click", async() => await this.handleAttendPartyClick());
        footerDiv.appendChild(attendPartyButton);
    }

    async handleAttendPartyClick() {
        const attendPartyRequest = 
        await fetch (serverUrl + "/Party/attend/" + this.party["id"] + "/" + localStorage.getItem("id"), {
            method: "POST"
        });

        if (!attendPartyRequest.ok) return;

        alert("Karta za zurku uzeta");

        this.container.parentNode.removeChild(this.container);        
    }
}   