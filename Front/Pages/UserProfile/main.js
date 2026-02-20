import { serverUrl } from "../../config.js";
import { prefix64Encoded } from "../../constants.js";

export class UserProfilePage {

    constructor() {
        this.usernameInput = document.body.querySelector(".username-input");
        this.emailInput = document.body.querySelector(".email-input");
        this.passwordInput = document.body.querySelector(".password-input");
        this.avatarInput = document.body.querySelector(".avatar-input");

        this.avatarButton = document.body.querySelector(".avatar-button");
        this.updateButton = document.body.querySelector(".update-button");

        this.avatarImage = document.body.querySelector(".avatar-image");

        this.usernameInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.emailInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.passwordInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.avatarInput.addEventListener("change", () => this.handleAvatarChange());

        this.avatarButton.addEventListener("click", () => this.handleAvatarClick());
        this.updateButton.addEventListener("click", async() => await this.handleUpdateClick());

        this.user = {username: "", email: "", password: "", avatar: ""};

        window.addEventListener("DOMContentLoaded", async() => {
            const userInfoRequest = await fetch (serverUrl + "/User/info/" + localStorage.getItem("id"));

            if (!userInfoRequest.ok) return;

            this.user = await userInfoRequest.json();

            this.usernameInput.setAttribute("value", this.user["username"]);
            this.emailInput.setAttribute("value", this.user["email"]);
            this.passwordInput.setAttribute("value", this.user["password"]);
            this.avatarImage.setAttribute("src", prefix64Encoded + this.user["avatar"]);
        });
    }

    handleInputChange(ev) {
        this.user[ev.target.name] = ev.target.value;
    }

    handleAvatarChange() {
        const file = this.avatarInput.files[0];

        const reader = new FileReader();

        reader.onload = ev => {
            this.user.avatar = ev.target.result.replace(/^data:(.*,)?/, '');
        }

        reader.readAsDataURL(file);
    }

    handleAvatarClick() {
        this.avatarInput.click();
    }

    async handleUpdateClick() {
        const updateProfileRequest = await fetch (serverUrl + "/User/update/" + localStorage.getItem("id"), {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.user)
        });

        if (!updateProfileRequest.ok) return;

        alert("Profil je azuriran");
    }
}

const userProfilePage = new UserProfilePage();