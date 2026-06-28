import { useEffect } from "react";
import { kcSanitize } from "keycloakify/lib/kcSanitize";
import type { TemplateProps } from "keycloakify/login/TemplateProps";
import { getKcClsx } from "keycloakify/login/lib/kcClsx";
import { useSetClassName } from "keycloakify/tools/useSetClassName";
import { useInitialize } from "keycloakify/login/Template.useInitialize";
import type { I18n } from "./i18n";
import type { KcContext } from "./KcContext";

export default function Template(props: TemplateProps<KcContext, I18n>) {
    const {
        displayMessage = true,
        documentTitle,
        bodyClassName,
        kcContext,
        i18n,
        doUseDefaultCss,
        classes,
        children
    } = props;

    const { kcClsx } = getKcClsx({ doUseDefaultCss, classes });
    const { msgStr } = i18n;
    const { realm, message, isAppInitiatedAction } = kcContext;

    useEffect(() => {
        document.title = documentTitle ?? msgStr("loginTitle", realm.displayName || realm.name);
    }, [documentTitle, realm.displayName, realm.name, msgStr]);

    useSetClassName({
        qualifiedName: "html",
        className: kcClsx("kcHtmlClass")
    });

    useSetClassName({
        qualifiedName: "body",
        className: bodyClassName ?? kcClsx("kcBodyClass")
    });

    const { isReadyToRender } = useInitialize({ kcContext, doUseDefaultCss });

    if (!isReadyToRender) {
        return null;
    }

    const alertClass =
        message?.type === "error"
            ? "kcAlertError"
            : message?.type === "success"
                ? "kcAlertSuccess"
                : message?.type === "warning"
                    ? "kcAlertWarning"
                    : message?.type === "info"
                        ? "kcAlertInfo"
                        : "";

    return (
        <div className="login-page-container">
            {/* Main Center Card Wrapper */}
            <div className="login-card-wrapper">
                {/* Custom Branding Header inside the card */}
                <div className="brand-header">
                    <div className="brand-logo-container">
                        <svg
                            width="96"
                            height="72"
                            viewBox="0 0 96 72"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                            className="brand-logo-svg"
                        >
                            <defs>
                                <linearGradient id="chevronGrad" x1="45" y1="36" x2="85" y2="36" gradientUnits="userSpaceOnUse">
                                    <stop offset="0%" stop-color="#0066FF" />
                                    <stop offset="100%" stop-color="#33B3FF" />
                                </linearGradient>
                            </defs>
                            {/* Pixelated/floating square nodes on trailing edge */}
                            <rect x="22" y="24" width="3.5" height="3.5" fill="#33B3FF" opacity="0.4" />
                            <rect x="22" y="36" width="3.5" height="3.5" fill="#0066FF" opacity="0.6" />
                            <rect x="22" y="48" width="3.5" height="3.5" fill="#0066FF" opacity="0.3" />

                            <rect x="28" y="18" width="3.5" height="3.5" fill="#33B3FF" opacity="0.5" />
                            <rect x="28" y="30" width="3.5" height="3.5" fill="#33B3FF" opacity="0.9" />
                            <rect x="28" y="42" width="3.5" height="3.5" fill="#0066FF" opacity="0.5" />

                            <rect x="34" y="24" width="3.5" height="3.5" fill="#33B3FF" />
                            <rect x="34" y="36" width="3.5" height="3.5" fill="#0066FF" />
                            <rect x="34" y="48" width="3.5" height="3.5" fill="#0066FF" opacity="0.7" />

                            <rect x="40" y="30" width="3.5" height="3.5" fill="#0066FF" />
                            <rect x="40" y="42" width="3.5" height="3.5" fill="#0066FF" />

                            {/* Main Triangle / Chevron Play icon layers */}
                            {/* Back layer (offset left and darker) */}
                            <path
                                d="M 44.5 15.5 L 75 36.5 L 44.5 57.5 L 53.5 36.5 Z"
                                fill="#004AD6"
                                opacity="0.75"
                            />
                            {/* Front layer */}
                            <path
                                d="M 49.5 14.5 L 81 36.5 L 49.5 58.5 L 57.5 36.5 Z"
                                fill="url(#chevronGrad)"
                            />
                        </svg>
                    </div>
                    <h1 className="brand-title">
                        <span className="brand-title-blue">SKB.</span>
                        <span className="brand-title-gray">VideoMultiplexer</span>
                    </h1>
                    <p className="brand-subtitle">Stream. Combine, Deliver</p>
                </div>

                {/* Display Warning/Error Messages */}
                {displayMessage && message !== undefined && (message.type !== "warning" || !isAppInitiatedAction) && (
                    <div className={`kcAlert ${alertClass}`}>
                        <div className="alert-content">
                            <span
                                dangerouslySetInnerHTML={{
                                    __html: kcSanitize(message.summary)
                                }}
                            />
                        </div>
                    </div>
                )}

                {/* Form Content (Login page contents) */}
                <div className="card-form-content">
                    {children}
                </div>
            </div>

            {/* Custom Branding Footer below the card */}
            <footer className="login-footer">
                <div className="footer-logo-circle">
                    <svg
                        width="18"
                        height="18"
                        viewBox="0 0 24 24"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                        className="footer-logo-svg"
                    >
                        <path
                            d="M 6.5 4.5 L 19 12 L 6.5 19.5 L 10 12 Z"
                            fill="#0066FF"
                        />
                    </svg>
                </div>
                <div className="footer-text-container">
                    <div className="footer-brand-name">SKB VideoMultiplexer</div>
                    <div className="footer-copyright-text">© 2024 All rights reserved.</div>
                </div>
            </footer>
        </div>
    );
}
