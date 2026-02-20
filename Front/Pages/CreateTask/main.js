import { serverUrl } from "../../config.js"

export class CreateTaskPage {
    constructor() {
        this.parties = [];

        this.choosePartySE = document.body.querySelector(".choose-party-se");
        this.taskNameInput = document.body.querySelector(".task-name-input");
        this.taskDescriptionInput = document.body.querySelector(".task-description-input");
        this.createTaskButton = document.body.querySelector(".create-task-button");

        this.task = {name: "", description: ""};
        this.partyId = null;

        this.choosePartySE.addEventListener("sl-input", (ev) => this.partyId = Number.parseInt(ev.target.value));
        this.taskNameInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.taskDescriptionInput.addEventListener("sl-input", ev => this.handleInputChange(ev));
        this.createTaskButton.addEventListener("click", async() => await this.createTaskClick());
    }

    async draw() {
        const partiesRequest = await fetch (serverUrl + "/Party/parties-names/" + localStorage.getItem("id"));

        if (!partiesRequest.ok) return;

        this.parties = await partiesRequest.json();

        this.parties.forEach(p => {
            const option = document.createElement("sl-option");
            option.setAttribute("value", p["id"]);
            option.textContent = p["name"];
            this.choosePartySE.appendChild(option);
        });
    }

    async createTaskClick() {
        const createTaskRequest =
         await fetch (serverUrl + "/Task/create/" + localStorage.getItem("id") + "/" + this.partyId, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.task)
         });

         if (!createTaskRequest.ok) return;

         alert("Task je uspesno napravljen");
    }

    handleInputChange(event) {
        this.task[event.target.name] = event.target.value;
    }
}

const createTaskPage = new CreateTaskPage();
await createTaskPage.draw();