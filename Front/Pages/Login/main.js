import { baseUrl, serverUrl } from "../../config.js";


export class LoginPage {
    constructor() {
        this.usernameInput = document.body.querySelector(".username-input");
        this.passwordInput = document.body.querySelector(".password-input");
        this.loginButton = document.body.querySelector(".login-button");
        this.registerButton = document.body.querySelector(".register-button");

        this.user = {username: "", password: ""};

        this.usernameInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.passwordInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.loginButton.addEventListener("click", async () => await this.handleLoginClick());
        this.registerButton.addEventListener("click", () => window.location = "../Register/index.html");
    }

    handleInputChange(ev) {
        this.user[ev.target.name] = ev.target.value;
    }

    async handleLoginClick() {
        console.log("OK")
        const userLoginRequest = await fetch (serverUrl + "/User/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.user)
        });
       
        if (!userLoginRequest.ok) {
            //mozda treba izmena kasnije ovde
            console.log("ERROR: Login..");
            return;
        }

        localStorage.clear();
        const id = Number.parseInt(await userLoginRequest.json());
        localStorage.setItem("id", id);
        window.location = "../Dashboard/index.html";
    }
}

const loginPage = new LoginPage();