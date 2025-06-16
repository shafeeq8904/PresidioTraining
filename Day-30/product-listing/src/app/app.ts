import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { Menu } from "./menu/menu";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  styleUrl: './app.css',
  imports: [Menu,RouterModule]
})
export class App {
  protected title = 'product-listing';
}
