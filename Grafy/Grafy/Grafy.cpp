// Grafy.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <omp.h>

using namespace std;
bool zmienna = true;
vector<vector<float>> graf;

vector<vector<float>> wczytaj(string nazwa_pliku)
{
	ifstream plik;
	plik.open(nazwa_pliku.c_str());
	string buf;
	plik >> buf;
	vector<vector<float>> tab;
	int size = stoi(buf);
	for (int i = 0; i < size; i++)
	{
		vector<float> wiersz;
		for (int j = 0; j < size; j++)
		{
			plik >> buf;
			wiersz.push_back(stof(buf));
		}
		tab.push_back(wiersz);
	}

	return tab;
}

int Silnia(int x)
{
	int wynik = 1;
	for (int i = 0; i <= x; i++)
	{
		wynik *= i;
	}
	return wynik;
}

std::vector<int> kth_perm(long long k, std::vector<int> V)
{
	long long int index;
	long long int next;
	std::vector<int> new_v;
	while (V.size())
	{
		index = k / Silnia(V.size() - 1);
		new_v.push_back(V.at(index));

		next = k % Silnia(V.size() - 1);
		V.erase(V.begin() + index);
		k = next;
	}
	return new_v;
}

/* Algorytm sekwencyjny */
void komiwojazer_sekwencyjnie()
{
	int liczbamiast = graf.size();
	int* indeksy = new int[liczbamiast + 1]; // Tablica przechowuj¹ca indeksy miast do tworzenia kolejnych permutacji
	int *permutacja = new int[liczbamiast + 1]; // Tablica przechowuj¹ca permutacjê indeksów miast, w których droga jest najszybsza/najtañsza
	float koncowykoszt = 9999; // zmienna przechowuj¹ca koñcowy koszt drogi
	for (int i = 0; i < liczbamiast + 1; i++) // pêtla tworz¹ca tablicê indeksów miast, gdzie indeks pocz¹tkowy i koñcowy zawiera  pierwsze miasto (0)
	{
		if (i == liczbamiast)
		{
			indeksy[i] = 0;
		}
		else
		{
			indeksy[i] = i;
		}
	}
	double czas = omp_get_wtime(); // Rozpoczêcie mierzenia czasu pracy algorytmu

	while (zmienna == true)
	{
		float koszt = 0.0;
		zmienna = next_permutation(indeksy + 1, indeksy + liczbamiast); // kolejna permutacja miast (pierwszy i ostatni indeks zostaj¹ takie same)
		for (int i = 0; i < liczbamiast; i++)
		{
			float droga = graf[indeksy[i]][indeksy[i + 1]]; // koszt drogi miêdzymiastowej
			if (droga == 0.0)
			{
				break; // Przerwanie pêtli, poniewa¿ droga nie jest mo¿liwa (próba dostania siê z miasta x do miasta x)
			}
			else
			{
				koszt += droga; // Dodawanie do kosztu ca³ej drogi kolejne koszty miêdzymiastowe
			}			
		}
		if (koszt < koncowykoszt)
		{
			for (int i = 0; i < liczbamiast + 1; i++) permutacja[i] = indeksy[i]; // Zapis najkrótszej permutacji do tablicy (permutacja)
			koncowykoszt = koszt; // Podmiana koñcowego kosztu na koszt najkrótszy
		}
	}
	cout.precision(8);
	czas = omp_get_wtime() - czas; // Zakoñczenie odmierzania czasu
	cout << "Najtansza droga: " << koncowykoszt << endl;
	cout << "Permutacja: ";
	for (int i = 0; i < liczbamiast + 1; i++)
	{
		cout << permutacja[i] << " ";
	}
	cout << endl << "Czas dzialania algorytmu: " << czas << " sekund" << endl;
}

void komiwojazer_rownolegle()
{
	zmienna = true;
	int liczbamiast = graf.size();
	vector<int> indeksy; // Tablica przechowuj¹ca indeksy miast do tworzenia kolejnych permutacji
	int *permutacja = new int[liczbamiast + 1]; // Tablica przechowuj¹ca permutacjê indeksów miast, w których droga jest najszybsza/najtañsza
	float koncowykoszt = 9999; // zmienna przechowuj¹ca koñcowy koszt drogi
	for (int i = 0; i < liczbamiast + 1; i++) // pêtla tworz¹ca tablicê indeksów miast, gdzie indeks pocz¹tkowy i koñcowy zawiera  pierwsze miasto (0)
	{
		if (i == liczbamiast)
		{
			indeksy.push_back(0);
		}
		else
		{
			indeksy.push_back(i);
		}
	}
	double czas = omp_get_wtime(); // Rozpoczêcie mierzenia czasu pracy algorytmu
	float koszt = 0.0;
#pragma omp parallel num_threads(4)
	{				
		// poczatek permutacji dla watku
		long long int start = (liczbamiast * omp_get_thread_num()) / omp_get_num_threads();
		// koniec permutacji dla watku
		long long int end = 10;
		if (omp_get_thread_num() == (omp_get_num_threads() - 1))
		{
			end = ((liczbamiast*(omp_get_thread_num() + 1)) / omp_get_num_threads());
		}		

		cout << "watek " << omp_get_thread_num() << " start: " << start << endl;
		cout << "watek " << omp_get_thread_num() << " stop: " << start << endl;

		indeksy = kth_perm(start, indeksy); // perm is your list of permutations
		cout << "watek " << omp_get_thread_num() << "przypisuje k-ta permutacje" << endl;

		for (int j = start; j < end; ++j)
		{
			float droga = graf[indeksy[j]][indeksy[j + 1]]; // koszt drogi miêdzymiastowej
			if (droga == 0.0)
			{
				break; // Przerwanie pêtli, poniewa¿ droga nie jest mo¿liwa (próba dostania siê z miasta x do miasta x)
			}
			else
			{
				koszt += droga; // Dodawanie do kosztu ca³ej drogi kolejne koszty miêdzymiastowe
			}
			next_permutation(indeksy.begin(), indeksy.end());

			if (koszt < koncowykoszt)
			{
				//for (i = 0; i < liczbamiast + 1; i++) permutacja[i] = indeksy[i]; // Zapis najkrótszej permutacji do tablicy (permutacja)
				koncowykoszt = koszt; // Podmiana koñcowego kosztu na koszt najkrótszy
			}
		}
	}

	cout.precision(8);
	czas = omp_get_wtime() - czas; // Zakoñczenie odmierzania czasu
	cout << "Najtansza droga: " << koncowykoszt << endl;
	cout << "Permutacja: ";
	for (int i = 0; i < liczbamiast + 1; i++)
	{
		cout << permutacja[i] << " ";
	}
	cout << endl << "Czas dzialania algorytmu: " << czas << " sekund" << endl;
}

int _tmain(int argc, _TCHAR* argv[])
{
	auto tablica = wczytaj("we10.txt");

	graf = tablica;
	//cout << "Algorytm sekwencyjny: " << endl;
	//komiwojazer_sekwencyjnie();

	cout << endl << "Algorytm zrownoleglony: " << endl;
	komiwojazer_rownolegle();

	system("pause");
	return 0;
}