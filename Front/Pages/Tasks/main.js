import { serverUrl } from "../../config.js"
import { Task } from "./task.js";

export class TasksPage {
    constructor() {
        this.container = document.body.querySelector(".container");
        //this.tasksTree = document.body.querySelector(".tasks-tree");
        this.tasks = [];


    }

    async draw() {
        const tasksRequest = await fetch (serverUrl + "/Task/my-tasks/" + localStorage.getItem("id"));

        if (!tasksRequest.ok) return;

        this.tasks = await tasksRequest.json();
        console.log(this.tasks);
        this.tasks.forEach(t => {
            const task = new Task(t);
            //task.draw(this.tasksTree);
            task.draw(this.container);
        });
    }
}

const tasksPage = new TasksPage();
await tasksPage.draw();