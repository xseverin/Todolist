import React from "react";

interface HeaderProps {
    mainTitle: string;
    subTitle: string;
    linkText: string;
    linkUrl: string;
    subTitle1?: string;
    linkText1?: string;
    linkUrl1?: string;
}

function Header({
                    mainTitle,
                    subTitle,
                    linkText,
                    linkUrl,
                    subTitle1,
                    linkText1,
                    linkUrl1,
                }: HeaderProps) {
    return (
        <div>
            <div className="text-center md:text-left">
                <h1 className="title-header mb-2">
                    <span>{mainTitle}</span>
                    <span className="dot">.</span>
                </h1>
                <span className="font-bold text-sm text-gray-700 dark:text-gray-300">
                    <span>
                        {subTitle}{" "}
                        <span className="text-fblue-100 px-2 py-1 rounded relative group">
    <a href={linkUrl} className="relative text-current">
        {linkText}
        <span
            className="absolute bottom-0 left-0 w-0 h-[1px] bg-blue-500 group-hover:w-full"></span>
    </a>
</span>

                    </span>
                </span>
                {subTitle1 && linkText1 && linkUrl1 && (
                    <>
                        <br/>
                        <span className="font-bold text-sm text-gray-700 dark:text-gray-300">
                            <span>
                                {subTitle1}{" "}
                                <span className="text-fblue-100 cursor-pointer transition-transform 
                                    duration-300 transform hover:scale-110 
                                    hover:text-white hover:bg-fblue-100 px-2 py-1 rounded">
                                    <a href={linkUrl1}>{linkText1}</a>
                                </span>
                            </span>
                        </span>
                    </>
                )}
            </div>
        </div>
    );
}

export default React.memo(Header);
