import { Component, OnInit } from '@angular/core';
import { Product } from '../interfaces/product';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Recipes } from '../services/recipes';

@Component({
  selector: 'app-home',
  imports: [FormsModule, CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {

  dataRecipes: any[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private _recipes: Recipes) {
  }
  ngOnInit(): void {
    this._recipes.getRecipes().subscribe({
      next: (data) => {
        this.dataRecipes  = data.data.recipes;
        console.log(this.dataRecipes);
        
        this.isLoading = false;
      }, error: (err) => {
        console.log(err);
        this.errorMessage = 'failed to load recipes';
        this.isLoading = false;
      }
    });
  }





  products: Product[] = [
    {
      id: 1,
      name: 'Wireless Mouse',
      description: 'Ergonomic wireless mouse with adjustable DPI and silent clicks.',
      price: 25.99,
      category: 'Electronics',
      stock: 120,
      imageUrl: '../../assets/images/7.jpg',
      rating: 4.5,
      isAvailable: true,
      isOnSale: true,
      isNewArrival: false,
    },
    {
      id: 2,
      name: 'Mechanical Keyboard',
      description: 'RGB mechanical keyboard with blue switches and durable keycaps.',
      price: 79.99,
      category: 'Electronics',
      stock: 60,
      imageUrl: '../../assets/images/8.jpg',
      rating: 4.8,
      isAvailable: false,
      isOnSale: false,
      isNewArrival: true,
    },
    {
      id: 3,
      name: 'Gaming Headset',
      description: 'Surround sound gaming headset with noise-cancelling microphone.',
      price: 59.99,
      category: 'Accessories',
      stock: 45,
      imageUrl: '../../assets/images/9.jpg',
      rating: 4.6,
      isAvailable: false,
      isOnSale: true,
      isNewArrival: false,
    },
    {
      id: 4,
      name: 'USB-C Charger',
      description: 'Fast-charging USB-C wall adapter compatible with multiple devices.',
      price: 19.99,
      category: 'Electronics',
      stock: 200,
      imageUrl: '../../assets/images/10.jpg',
      rating: 4.3,
      isAvailable: true,
      isOnSale: false,
      isNewArrival: true,
    },
    {
      id: 5,
      name: 'Smartwatch',
      description: 'Fitness tracking smartwatch with heart rate monitor and GPS.',
      price: 129.99,
      category: 'Wearables',
      stock: 30,
      imageUrl: '../../assets/images/11.jpg',
      rating: 4.7,
      isAvailable: false,
      isOnSale: true,
      isNewArrival: false,
    },
    {
      id: 6,
      name: 'Bluetooth Speaker',
      description: 'Portable waterproof Bluetooth speaker with deep bass sound.',
      price: 49.99,
      category: 'Audio',
      stock: 80,
      imageUrl: '../../assets/images/12.jpg',
      rating: 4.4,
      isAvailable: true,
      isOnSale: false,
      isNewArrival: true,
    },
  ];



}
