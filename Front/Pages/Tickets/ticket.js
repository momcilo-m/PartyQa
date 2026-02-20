import { prefix64Encoded } from "../../constants.js";
import { serverUrl } from "../../config.js"

export class Ticket {
    constructor(ticket) {
        this.container = null;
        this.ticket = ticket;
    }

    draw(container) {
        const card = document.createElement("sl-card");
        this.container = card;
        card.classList.add("card-overview");
        container.appendChild(card);

        const cardImg = document.createElement("img");
        cardImg.classList.add("card-image");
        cardImg.setAttribute("slot", "image");
        cardImg.setAttribute("src", prefix64Encoded + this.ticket.image);
        cardImg.setAttribute("alt", "ticket image");
        card.appendChild(cardImg);

        const ticketNameLab = document.createElement("strong");
        ticketNameLab.textContent = this.ticket.name;
        card.appendChild(ticketNameLab);
        card.appendChild(document.createElement("br"));
        card.appendChild(document.createElement("br"));

        const ticketCityLab = document.createElement("label");
        ticketCityLab.textContent = "City: " + this.ticket.city;
        card.appendChild(ticketCityLab);
        card.appendChild(document.createElement("br"));

        const ticketAddressLab = document.createElement("label");
        ticketAddressLab.textContent = "Address: " + this.ticket.address;
        card.appendChild(ticketAddressLab);

        const footerDiv = document.createElement("div");
        footerDiv.classList.add("footer-div");
        footerDiv.setAttribute("slot", "footer");
        card.appendChild(footerDiv);

        const unattendButton = document.createElement("sl-button");
        unattendButton.textContent = "Unattend";
        unattendButton.addEventListener("click", async() => await this.unAttendClick());
        footerDiv.appendChild(unattendButton);
    }

    async unAttendClick() {
        const unattendRequest = await fetch (serverUrl + "/Party/unattend/" + this.ticket["id"] + "/" + localStorage.getItem("id"), {
            method: "DELETE"
        });

        if (!unattendRequest.ok) return;

        alert("Odlazak otkazan");

        this.container.parentNode.removeChild(this.container);
    }
}