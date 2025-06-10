import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { First } from "./first/first";

@Component({
  selector: 'app-root',
  imports: [ First],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'myApp';
}