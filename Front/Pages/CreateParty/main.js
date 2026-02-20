import { serverUrl } from "../../config.js";


export class CreatePartyPage {
    constructor() {
        this.nameInput = document.body.querySelector(".name-input");
        this.cityInput = document.body.querySelector(".city-input");
        this.addressInput = document.body.querySelector(".address-input");
        this.imageInput = document.body.querySelector(".image-input");

        this.nameInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.cityInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.addressInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.imageInput.addEventListener("change", () => this.handleImageChange());

        this.imageButton = document.body.querySelector(".image-button");
        this.imageButton.addEventListener("click", () => this.handleImageClick());
        this.createPartyButton = document.body.querySelector(".create-party-button");
        this.createPartyButton.addEventListener("click", async() => await this.handleCreatePartyClick());

        this.party = {name: "", city: "", address: "", image: ""};
    }

    handleInputChange(ev) {
        this.party[ev.target.name] = ev.target.value;
    }

    handleImageChange() {
        const file = this.imageInput.files[0];

        const reader = new FileReader();

        reader.onload = ev => {
            this.party.image = ev.target.result.replace(/^data:(.*,)?/, '');
        }

        reader.readAsDataURL(file);
    }

    handleImageClick() {
        this.imageInput.click();
    }

    async handleCreatePartyClick() {
        const createPartyRequest = await fetch (serverUrl + "/Party/create/" + localStorage.getItem("id"), {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.party)
        });

        if (!createPartyRequest.ok) return;

        alert("Zurka napravljena");
    }
}

const createPartyPage = new CreatePartyPage();