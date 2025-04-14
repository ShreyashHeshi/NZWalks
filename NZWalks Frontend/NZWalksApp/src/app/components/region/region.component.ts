import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { RegionService } from '../../services/region.service';
import { RegionDTO } from './RegionDTO';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

declare const bootstrap: any;

@Component({
  selector: 'app-region',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './region.component.html',
  styleUrls: ['./region.component.css']
})
export class RegionComponent implements OnInit, AfterViewInit {

  paginatedRegions: RegionDTO[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 9;
  totalItems: number = 0;
  totalPages: number = 1;
  regions: RegionDTO[] = [];
  isLoading = false;
  error: string | null = null;
  selectedImageUrl: string | null = null;

  // Create Region Variables
  showCreateForm = false;
  isCreating = false;
  createError: string | null = null;
  regionForm: FormGroup;

  // Update Region Variables
  updateForm: FormGroup;
  isUpdating = false;
  selectedRegionId: string | null = null;

  // Delete Region Variables
  isDeleting = false;
  regionToDelete: RegionDTO | null = null;

  // Modal references
  @ViewChild('updateRegionModal') updateModalElement!: ElementRef;
  @ViewChild('deleteConfirmationModal') deleteModalElement!: ElementRef;
  @ViewChild('imageModal') imageModalElement!: ElementRef;

  public updateModalInstance: any;
  public deleteModalInstance: any;
  public imageModalInstance: any;

  constructor(
    private regionService: RegionService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) {
    this.regionForm = this.fb.group({
      code: ['', [Validators.required, Validators.maxLength(5)]],
      name: ['', Validators.required],
      regionImageUrl: ['']
    });

    this.updateForm = this.fb.group({
      code: ['', [Validators.required, Validators.maxLength(5)]],
      name: ['', Validators.required],
      regionImageUrl: ['']
    });
  }

  ngOnInit(): void {
    this.fetchRegions();
  }

  ngAfterViewInit(): void {
    // Initialize modals after view is initialized
    this.updateModalInstance = new bootstrap.Modal(this.updateModalElement.nativeElement);
    this.deleteModalInstance = new bootstrap.Modal(this.deleteModalElement.nativeElement);
    this.imageModalInstance = new bootstrap.Modal(this.imageModalElement.nativeElement);
  }

  fetchRegions(): void {

    this.isLoading = true;
    this.error = null;

    // this.regionService.getAllRegions(this.currentPage, this.itemsPerPage).subscribe({
    //   next: (response) => {
    //     // Use paginatedRegions directly since server is handling pagination
    //     this.paginatedRegions = response || [];
    //     this.totalItems = response.totalCount || 0;
    //     this.totalPages = Math.ceil(this.totalItems / this.itemsPerPage) || 1;
    //     this.isLoading = false;
    //     // Remove updatePaginatedData() call since we don't need client-side slicing
    //   },
    //   error: (err) => {
    //     this.error = 'Failed to load regions. Please try again later.';
    //     this.isLoading = false;
    //     this.paginatedRegions = []; // Reset on error
    //     this.toastr.error('Failed to load regions', 'Error');
    //     console.error('Error fetching regions:', err);
    //   }
    // });

    this.regionService.getAllRegions(this.currentPage, this.itemsPerPage).subscribe({
      next: (response) => {
        let regions = [];
        let totalItems = 0;

        if (Array.isArray(response)) {
          regions = response;
          totalItems = response.length;
        } else {
          regions = response?.regions || [];
          totalItems = response?.totalCount || 0;
        }

        this.paginatedRegions = regions;
        this.totalItems = totalItems;
        this.totalPages = Math.ceil(this.totalItems / this.itemsPerPage);
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.paginatedRegions = [];
        this.error = 'Failed to load regions. Please try again later.';
        this.toastr.error('Failed to load regions', 'Error');
        console.log('Error fetching regions:', err);
      }
    });

  }

  // updatePaginatedData(): void {
  //   // For client-side pagination (if not using server-side)
  //   const startIndex = (this.currentPage - 1) * this.itemsPerPage;
  //   const endIndex = startIndex + this.itemsPerPage;
  //   this.regions = this.regions.slice(startIndex, endIndex);
  // }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.fetchRegions();
  }

  // Add this method to your component class
  getPages(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5; // Show maximum 5 page numbers at a time
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = startPage + maxVisiblePages - 1;

    if (endPage > this.totalPages) {
      endPage = this.totalPages;
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }



  // Method to open the image modal
  viewImage(imageUrl: string): void {
    this.selectedImageUrl = imageUrl;
    this.imageModalInstance?.show();
  }

  // Method to close the image modal
  closeImageModal(): void {
    this.selectedImageUrl = null;
    this.imageModalInstance?.hide();
  }


  onSubmit(): void {
    if (this.regionForm.invalid) return;

    this.isCreating = true;
    this.createError = null;

    this.regionService.createRegion(this.regionForm.value).subscribe({
      next: () => {
        this.showCreateForm = false;
        this.isCreating = false;
        this.regionForm.reset();
        this.fetchRegions();
        this.toastr.success('Region created successfully', 'Success');
      },
      error: (err) => {
        this.createError = 'Failed to create region. Please try again.';
        this.isCreating = false;
        this.toastr.error('Failed to create region', 'Error');
        console.error('Error creating region:', err);
      }
    });
  }

  // Add this property to your component
  isLoadingData = false;

  // Update your openUpdateModal method
  async openUpdateModal(region: RegionDTO): Promise<void> {
    try {
      this.isLoadingData = true;
      this.updateModalInstance?.show();

      // Set form values after a small delay to ensure modal is ready
      await new Promise(resolve => setTimeout(resolve, 50));

      this.selectedRegionId = region.id;
      this.updateForm.patchValue({
        code: region.code,
        name: region.name,
        regionImageUrl: region.regionImageUrl || ''
      });

      this.isLoadingData = false;
    } catch (error) {
      this.isLoadingData = false;
      this.toastr.error('Failed to load region data', 'Error');
      console.error('Error loading region data:', error);
    }
  }

  // updateRegion(): void {
  //   if (this.updateForm.invalid || !this.selectedRegionId) return;

  //   this.isUpdating = true;
  //   const updatedRegion = this.updateForm.value;

  //   this.regionService.updateRegion(this.selectedRegionId, updatedRegion).subscribe({
  //     next: () => {
  //       this.updateModalInstance?.hide();
  //       this.isUpdating = false;
  //       this.fetchRegions();
  //       this.toastr.success('Region updated successfully', 'Success');
  //     },
  //     error: (err) => {
  //       this.isUpdating = false;
  //       this.toastr.error('Failed to update region', 'Error');
  //       console.error('Error updating region:', err);
  //     }
  //   });
  // }

  // Add this method to close the modal properly
  closeUpdateModal(): void {
    this.updateModalInstance?.hide();
    this.updateForm.reset();
    this.selectedRegionId = null;
    this.isUpdating = false;
    this.isLoadingData = false;
  }

  // Modify your updateRegion method
  updateRegion(): void {
    if (this.updateForm.invalid || !this.selectedRegionId) {
      this.toastr.error('Please fill all required fields', 'Error');
      return;
    }

    this.isUpdating = true;
    const updatedRegion = this.updateForm.value;

    this.regionService.updateRegion(this.selectedRegionId, updatedRegion).subscribe({
      next: () => {
        this.toastr.success('Region updated successfully', 'Success');
        this.fetchRegions();
        this.closeUpdateModal(); // Use the new close method
      },
      error: (err) => {
        this.isUpdating = false;
        this.toastr.error('Failed to update region', 'Error');
        console.error('Error updating region:', err);
      }
    });
  }

  // In your confirmDelete method - modify it like this:
  confirmDelete(region: RegionDTO): void {
    console.log('Region to delete:', region); // Debug log
    if (!region || !region.id) {
      console.error('Invalid region or missing ID:', region);
      return;
    }

    // Create a new object to ensure we keep all properties
    this.regionToDelete = {
      id: region.id,
      code: region.code,
      name: region.name,
      regionImageUrl: region.regionImageUrl
    };

    console.log('Region set for deletion:', this.regionToDelete); // Debug log
    this.deleteModalInstance?.show();
  }

  // Update your deleteRegion method:
  deleteRegion(): void {
    console.log('Attempting delete with:', this.regionToDelete); // Debug log

    if (!this.regionToDelete || !this.regionToDelete.id) {
      console.error('Cannot delete - no valid region selected');
      this.toastr.error('No region selected for deletion', 'Error');
      return;
    }

    this.isDeleting = true;

    this.regionService.deleteRegion(this.regionToDelete.id).subscribe({
      next: () => {
        console.log('Delete successful');
        this.deleteModalInstance.hide();
        this.isDeleting = false;
        this.fetchRegions();
        this.toastr.success(`Region deleted successfully`, 'Success');
        this.regionToDelete = null;
      },
      error: (err) => {
        console.error('Delete failed:', err);
        this.isDeleting = false;
        this.toastr.error('Failed to delete region', 'Error');
      }
    });
  }

}