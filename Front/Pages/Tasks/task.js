import { serverUrl } from "../../config.js";


export class Task {

    constructor(task) {
        this.container = null;
        this.task = task;
    }

    draw(container) {

        const card = document.createElement("sl-card");
        this.container = card;
        card.classList.add("party-card");
        container.appendChild(card);

        const partyNameLab = document.createElement("strong");
        partyNameLab.textContent = "Party: " + this.task["partyName"];
        card.appendChild(partyNameLab);
        card.appendChild(document.createElement("br"));

        this.task["tasks"].forEach(t => {
            const taskCard = document.createElement("sl-card");
            taskCard.classList.add("task-card")
            card.appendChild(taskCard);

            const taskName = document.createElement("strong");
            taskName.textContent = "Task: " + t["taskName"];
            taskCard.appendChild(taskName);

            taskCard.appendChild(document.createElement("br"));

            const taskDescription = document.createElement("small");
            taskDescription.textContent = "Description: " + t["taskDescription"];
            taskCard.appendChild(taskDescription);

            const footerDiv = document.createElement("div");
            taskCard.appendChild(footerDiv);
            footerDiv.classList.add("footer-div");

            const editButton = document.createElement("sl-button");
            editButton.textContent = "Edit";
            editButton.classList.add("edit-button");
            editButton.addEventListener("click", async() => {
                const newTaskName = prompt("New task name");
                const newTaskDescription = prompt("New description");

                const taskEditRequest = await fetch(serverUrl + "/Task/edit/" + t["taskId"], {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({name: newTaskName, description: newTaskDescription})
                });

                if (!taskEditRequest.ok) return;

                alert("Task uspesno editovan");

                taskName.textContent = newTaskName;
                taskDescription.textContent = newTaskDescription;
            });
            footerDiv.appendChild(editButton);

            const removeButton = document.createElement("sl-button");
            removeButton.textContent = "Remove";
            removeButton.classList.add("remove-button");
            removeButton.addEventListener("click", async() => {
                const taskRemoveRequest = await fetch (serverUrl + "/Task/remove/" + t["taskId"], {
                    method: "DELETE"
                });

                if (!taskRemoveRequest.ok) return;

                alert("Task je uspesno obrisan");

                card.removeChild(taskCard);
            });
            footerDiv.appendChild(removeButton);
        });
    }
}