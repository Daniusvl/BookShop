using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class PhotoViewModel : BaseModelViewModel<PhotoModel, PhotoWrapper>
    {
        private readonly IUnitOfWork unitOfWork;

        public PhotoViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Collection = new() { new PhotoModel { } };
            SelectedItem = new(Collection[0]);

            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
            ShowPhotoCommand = new Command(param => true, ExecuteShowPhoto);
            UploadPhotoCommand = new Command(param => true, ExecuteUploadPhoto);
        }

        public override PhotoWrapper SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;

                if (selectedItem?.Model?.Id == 0)
                {
                    ShowPhotoCommandVisibility = Visibility.Hidden;
                    AddVisibility = Visibility.Visible;
                    UpdateVisibility = Visibility.Hidden;
                    DeleteVisibility = Visibility.Hidden;
                }
                else
                {
                    ShowPhotoCommandVisibility = Visibility.Visible;
                    AddVisibility = Visibility.Hidden;
                    UpdateVisibility = Visibility.Visible;
                    DeleteVisibility = Visibility.Visible;
                }
                FilePath = string.Empty;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public async void ExecuteLoad(object param)
        {
            LoadEnabled = false;

            await SafeRequestSend(async () =>
            {
                int selected_id = SelectedItem.Id;
                Collection = (await unitOfWork.PhotoRepository.GetAll()).ToList();
                Collection.Reverse();
                Collection.Insert(0, new PhotoModel { });
                Collection = new(Collection);
                SelectedItem = new(Collection.FirstOrDefault(p => p.Id == selected_id));
            });

            LoadEnabled = true;
        }

        public bool CanAdd(object param)
        {
            return !SelectedItem.HasErrors;
        }

        public async void ExecuteAdd(object param)
        {
            AddEnabled = false;

            await SafeRequestSend(async () =>
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("You must add photo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    AddEnabled = true;
                    return;
                }

                PhotoModel photo = await unitOfWork.PhotoRepository.Add(SelectedItem.Model);
                if (photo != null)
                {
                    await unitOfWork.PhotoRepository.UploadFile(FilePath, photo.Id);
                    Collection[0] = photo;
                    Collection.Insert(0, new PhotoModel { });
                    Collection = new(Collection);
                    SelectedItem = new(Collection[1]);
                }
            });

            AddEnabled = true;
        }

        public async void ExecuteUpdate(object param)
        {
            UpdateEnabled = false;

            await SafeRequestSend(async () =>
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to update this item {SelectedItem.Id}",
                        "Update?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int selected_id = SelectedItem.Id;
                    PhotoModel photo = await unitOfWork.PhotoRepository.Update(SelectedItem.Model);
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        await unitOfWork.PhotoRepository.UploadFile(FilePath, photo.Id);
                    }
                    PhotoModel old = Collection.FirstOrDefault(p => p.Id == selected_id);
                    int index = Collection.IndexOf(old);
                    Collection[index] = photo;
                    Collection = new(Collection);
                    SelectedItem = new(Collection[index]);
                }
            });

            UpdateEnabled = true;
        }

        public async void ExecuteDelete(object param)
        {
            DeleteEnabled = false;

            await SafeRequestSend(async () =>
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item {SelectedItem.Id}",
                    "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int selected_id = SelectedItem.Id;
                    await unitOfWork.PhotoRepository.Remove(selected_id);
                    PhotoModel photo = Collection.FirstOrDefault(p => p.Id == selected_id);
                    Collection.Remove(photo);
                    Collection = new(Collection);
                    SelectedItem = new(Collection[0]);
                }
            });

            DeleteEnabled = true;
        }

        public Action<List<byte>> ShowPhoto { get; set; }

        public async void ExecuteShowPhoto(object param)
        {
            await SafeRequestSend(async () =>
            {
                List<byte> bytes = (await unitOfWork.PhotoRepository.GetPhotoBytes(SelectedItem.Id)).ToList();
                ShowPhoto(bytes);
            });
        }

        private string filePath;
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public void ExecuteUploadPhoto(object param)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Png Files|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        public ICommand ShowPhotoCommand { get; }

        public ICommand UploadPhotoCommand { get; }

        private Visibility showPhotoCommandVisibility = Visibility.Hidden;
        public Visibility ShowPhotoCommandVisibility
        {
            get => showPhotoCommandVisibility;
            set
            {
                showPhotoCommandVisibility = value;
                OnPropertyChanged(nameof(ShowPhotoCommandVisibility));
            }
        }
    }
}
