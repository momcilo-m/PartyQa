import { serverUrl } from "../../config.js";
import { prefix64Encoded } from "../../constants.js"


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

        const editPartyButton = document.createElement("sl-button");
        editPartyButton.textContent = "Edit Party";
        editPartyButton.classList.add("edit-party-button");
        editPartyButton.addEventListener("click", async() => {
            const newPartyName = prompt("New party name");
            const newPartyCity = prompt("New party city");
            const newPartyAddress = prompt("New party address");

            const partyUpdateRequest = await fetch (serverUrl + "/Party/edit/" + this.party["id"], {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({name: newPartyName, city: newPartyCity, address: newPartyAddress, image: this.party["image"]})
            });

            if (!partyUpdateRequest.ok) return;

            alert("Zurka je izmenjena");

            this.party["name"] = newPartyName;
            this.party["city"] = newPartyCity;
            this.party["address"] = newPartyAddress;

            partyNameLab.textContent = newPartyName;
            partyCityLab.textContent = newPartyCity;
            partyAddressLab.textContent = newPartyAddress;

        });
        footerDiv.appendChild(editPartyButton);

        const cancelPartyButton = document.createElement("sl-button");
        cancelPartyButton.textContent = "Cancel Party";
        cancelPartyButton.classList.add("cancel-party-button");
        cancelPartyButton.setAttribute("aria-label", "Cancel Party");
        cancelPartyButton.ariaLabel = "Cancel Party";
        cancelPartyButton.addEventListener("click", async() => await this.handleCancelPartyClick());
        footerDiv.appendChild(cancelPartyButton);
    }

    async handleCancelPartyClick() {
        const cancelPartyRequest =
         await fetch (serverUrl + "/Party/cancel/" + this.party["id"], {
            method: "DELETE"
        });

        if (!cancelPartyRequest.ok) return;

        alert("Zurka je otkazana");

        this.container.parentNode.removeChild(this.container);        
    }
}   