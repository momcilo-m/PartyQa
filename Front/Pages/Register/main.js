import { serverUrl } from "../../config.js";

export class RegisterPage {
    constructor() {
        this.usernameInput = document.body.querySelector(".username-input");
        this.emailInput = document.body.querySelector(".email-input");
        this.passwordInput = document.body.querySelector(".password-input");
        this.avatarInput = document.body.querySelector(".avatar-input");

        this.avatarButton = document.body.querySelector(".avatar-button");
        this.loginButton = document.body.querySelector(".login-button");
        this.registerButton = document.body.querySelector(".register-button");

        this.usernameInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.emailInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.passwordInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.avatarInput.addEventListener("change", () => this.handleAvatarChange());

        this.avatarButton.addEventListener("click", () => this.handleAvatarClick());
        this.loginButton.addEventListener("click", () => window.location = "../Login/index.html");
        this.registerButton.addEventListener("click", async () => await this.handleRegisterClick());

        this.user = {username: "", email: "", password: "", avatar: ""};
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

    async handleRegisterClick() {
        const userRegisterRequest = await fetch (serverUrl + "/User/signup", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.user)
        });

        if (!userRegisterRequest.ok) {
            console.log("ERROR: Registracija neuspesna");
            return;
        }

        window.location = "../Login/index.html";
    }
}

const registerPage = new RegisterPage();