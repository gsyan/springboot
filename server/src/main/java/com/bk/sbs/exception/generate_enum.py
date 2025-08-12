import re

def generate_csharp_enum(java_file_path, output_file_path):
    # Java 파일 읽기
    with open(java_file_path, 'r', encoding='utf-8') as file:
        content = file.read()

    # enum 상수 파싱 (예: SUCCESS(0, "Success"))
    pattern = r'(\w+)\((\d+|Integer\.MAX_VALUE),\s*"([^"]+)"\)'  # Integer.MAX_VALUE 처리
    matches = re.findall(pattern, content)

    # C# enum 코드 생성
    csharp_code = "using System.Collections.Generic;\n\n public enum ServerErrorCode\n{\n"
    mapping_code = "public static class ErrorCodeMapping\n{\n    public static readonly Dictionary<ServerErrorCode, string> Messages = new Dictionary<ServerErrorCode, string>\n    {\n"
    for name, value, message in matches:
        # Integer.MAX_VALUE 처리
        if value == "Integer.MAX_VALUE":
            csharp_value = "int.MaxValue"
        else:
            csharp_value = value
        csharp_code += f"    {name} = {csharp_value},\n"
        mapping_code += f"        {{ ServerErrorCode.{name}, \"{message}\" }},\n"
    csharp_code += "}"
    mapping_code += "    };\n}"

    # C# 파일 쓰기
    with open(output_file_path, 'w', encoding='utf-8') as file:
        file.write(csharp_code + "\n\n" + mapping_code)

    print(f"C# enum file generated at: {output_file_path}")

if __name__ == "__main__":
    java_file_path = "ServerErrorCode.java"  # 서버의 Java 파일 경로
    output_file_path = "C:/bk/sbsClient/Assets/Scripts/ServerErrorCode.cs"  # 생성될 C# 파일 경로
    generate_csharp_enum(java_file_path, output_file_path)