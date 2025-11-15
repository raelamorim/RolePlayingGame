# UBS.Risk.Evaluator.Batch 🏦⚙️

Welcome to **UBS.Risk.Evaluator.Batch**! This project is responsible for processing and classifying trades automatically, logging, and generating classified outputs. The solution is organized into two projects:

1. **UBS.Risk.Evaluator.Batch** (main project)  
2. **UBS.Risk.Evaluator.Batch.Test** (unit testing project)

## Directory Structure 🌳

```mathematica
UBS.Risk.Evaluator.Batch
│   ├── Application
│   │    └── ...
│   ├── Domain
│   │    └── ...
│   ├── Infrastructure
│   │    └── ...
│   ├── Local
│   │    └── ...
│   ├── appsettings.json
│   ├── Dockerfile
│   └── Program.cs
│
└── UBS.Risk.Evaluator.Batch.Test
    └── ...
```

### Key Directories:

- **Application**  
  Contains the trade processing logic, such as the `TradeProcessor` class.

- **Domain**  
  Responsible for the project's business rules.

- **Infrastructure**  
  Responsible for support services, such as file reading and writing (`FileManager`).

- **Local**
  Example directory where input or output files are located.

- **appsettings.json**  
  Configuration file defining input and output paths, among other variables.

- **Dockerfile**  
  Allows the creation of a Docker image to run the project in a container.

- **UBS.Risk.Evaluator.Batch.Test**  
  Contains unit tests written in **xUnit**. Here you find `TradeProcessorTests.cs` and other test files.

## How to Run 🚀

1. **Clone the Repository**  
   ```bash
   git clone https://github.com/raelamorim/ubs-risk-evaluator-batch.git
   cd UBS.Risk.Evaluator.Batch
    ```

2. **Restore Dependencies**
    ```bash
    dotnet restore
    ```
3. **Run the Application**
    ```bash
    dotnet run --project .\UBS.Risk.Evaluator.Batch\UBS.Risk.Evaluator.Batch.csproj
    ```

    The application will use the configurations defined in appsettings.json. Ensure the file paths are correct.

## How to Run Tests 🧪

1. **Enter the Solution Directory**
Make sure you are in the root of the repository, where the .sln file is.

2. **Run Tests**
    ```bash
    dotnet test
    ```
    This will run all tests in UBS.Risk.Evaluator.Batch.Test and display the result in the console.

## How to Build the Docker Image 🐳

1. **Open project directory**
    ```bash
    cd UBS.Risk.Evaluator.Batch    
    ```

2. **Create the Image**
    ```bash
    docker build -t ubs-risk-evaluator .
    ```

2. **Run the Container**
    ```bash
    docker run --rm ubs-risk-evaluator
    ```
    Adjust volume mappings or environment variables as needed to point to input/output files.

## File and Directory Configuration 📂

* `appsettings.json`
    * `InputFilePath`: Path to the input file containing the trades.
    * `OutputFilePath`: Path to the output file where the classification will be saved.

* `Local/Input`
    * Example folder where input files can be placed for local processing.


## Contributing 🤝

1. Fork the repository.
2. Create your feature branch `git checkout -b feature/my-feature.`
3. Commit your changes: `git commit -m 'Add new feature'.`
4. Push to the branch: `git push origin feature/my-feature.`
5. Open a Pull Request for review.

## License 📄

The MIT License (MIT)

Copyright (c) 2025 Israel Amorim

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE
